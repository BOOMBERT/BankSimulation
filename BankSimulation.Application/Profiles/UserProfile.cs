using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Profiles
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, AuthUserDto>();
            CreateMap<AdminUpdateUserDto, User>();
            CreateMap<User, AdminUpdateUserDto>();
        }
    }
}
