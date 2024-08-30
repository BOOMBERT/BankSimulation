using AutoMapper;
using BankSimulation.Application.Dtos;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Profiles
{
    public sealed class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
        }
    }
}
