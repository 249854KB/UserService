using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.SyncDataServices.Grpc;
using UserService.SyncDataServices.Http;

namespace UserService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        

        public void ConfigureServices(IServiceCollection services)
        {
            if(_env.IsProduction())
            {
                Console.WriteLine("--> Using SQL DB");
                services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("UsersConn")));

           } else 
            {
                Console.WriteLine("--> Using In memory DB");
                services.AddDbContext<AppDbContext>(optionsAction =>
                optionsAction.UseInMemoryDatabase("InMemname"));
            }
            
            services.AddHttpClient<IForumDataClient, HttpForumDataClient>();
            services.AddHttpClient<IDogDataClient, HttpDogDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddScoped<IUserRepo, UserRepo>(); //IF they ask for IUser Repo we give them user repo
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService", Version = "v1" });
            });

            Console.WriteLine($"--> Forum Service Endpoint is {Configuration["ForumService"]}");
            Console.WriteLine($"--> Dog Service Endpoint is {Configuration["DogService"]}");

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcUserService>();
                endpoints.MapGet("/protocols/users.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protocols/users.proto"));
                });
            });
           PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}