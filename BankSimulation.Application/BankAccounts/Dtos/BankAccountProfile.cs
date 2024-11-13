using AutoMapper;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.BankAccounts.Dtos
{
    internal sealed class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
        }
    }
}
