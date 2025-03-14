using AutoMapper;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Roomify.GP.Service.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
