using System.Text;
using System.Text.Json;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public class HttpDogDataClient : IDogDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpDogDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendUserToDog(UserReadDto user)
        {
           var httpContent = new StringContent(
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["DogService"]}" , httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to DogService was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to DogService was ERROR");
            }
        }
    }
}