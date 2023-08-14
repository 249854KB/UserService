using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // Source -> Target
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserReadDto, UserPublishedDto>();
            CreateMap<User,GrpcUserModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src=>src.Id))
                .ForMember(dest => dest.Name,opt => opt.MapFrom(src=>src.Name))
                .ForMember(dest => dest.RankInSystem, opt => opt.MapFrom(src=>src.RankInSystem))
                .ForMember(dest => dest.NumberOfDogs, opt => opt.MapFrom(src=>src.NumberOfDogs));
        }
    }
}