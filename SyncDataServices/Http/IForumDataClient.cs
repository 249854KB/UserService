using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IForumDataClient
    {
        Task SendUserToForum(UserReadDto user);
    }
}