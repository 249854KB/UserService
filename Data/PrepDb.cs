using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Models;

namespace UserService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }

        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {   
                Console.WriteLine("--> Attempting migration to SQL");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run sql migrations: {ex.Message}");
                }
            }
            
            if(!context.Users.Any())
            {
                Console.WriteLine("--> Seeding Data ... ");
                context.Users.AddRange(
                    new User() {Name = "Pawel", RankInSystem = 2, NumberOfDogs = 1},
                    new User() {Name = "Adam", RankInSystem = 2, NumberOfDogs = 1}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have Data");
            }
        }
    }
}