using System.Text;
using System.Text.Json;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public class HttpForumDataClient : IForumDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpForumDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendUserToForum(UserReadDto user)
        {
           var httpContent = new StringContent(
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["ForumService"]}" , httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to ForumService was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to ForumService was ERROR");
            }
        }
    }
}