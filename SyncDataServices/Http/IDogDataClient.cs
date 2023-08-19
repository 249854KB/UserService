using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IDogDataClient
    {
        Task SendUserToDog(UserReadDto user);
    }
}