﻿using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
