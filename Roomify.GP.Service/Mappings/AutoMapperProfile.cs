﻿using AutoMapper;
using Roomify.GP.Core.DTOs.PortfolioPost;
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

            CreateMap<PortfolioPostDto, PortfolioPost>()
            .ForMember(dest => dest.ImagePath, options => options.Ignore())
            .ForMember(dest => dest.Id, options => options.Ignore())
            .ForMember(dest => dest.CreatedAt, options => options.Ignore())
            .ForMember(dest => dest.User, options => options.Ignore());

            CreateMap<PortfolioPost, PortfolioPostResponseDto>();



            CreateMap<PortfolioPostUpdateDto, PortfolioPost>()
            .ForMember(dest => dest.ImagePath, options => options.Ignore())
            .ForMember(dest => dest.Id, options => options.Ignore())
            .ForMember(dest => dest.CreatedAt, options => options.Ignore())
            .ForMember(dest => dest.UpdatedAt, options => options.Ignore())
            .ForMember(dest => dest.User, options => options.Ignore())
            .ForMember(dest => dest.UserId, options => options.Ignore());
        }
    }
}
