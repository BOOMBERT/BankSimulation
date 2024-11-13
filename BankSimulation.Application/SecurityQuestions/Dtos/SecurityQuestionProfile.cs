using AutoMapper;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.SecurityQuestions.Dtos
{
    internal sealed class SecurityQuestionProfile : Profile
    {
        public SecurityQuestionProfile()
        {
            CreateMap<SecurityQuestion, SecurityQuestionDto>();
        }
    }
}
