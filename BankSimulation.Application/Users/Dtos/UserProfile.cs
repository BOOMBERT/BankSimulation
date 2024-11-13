using AutoMapper;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Users.Dtos
{
    internal sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, CreateUserDto>();
        }
    }
}
