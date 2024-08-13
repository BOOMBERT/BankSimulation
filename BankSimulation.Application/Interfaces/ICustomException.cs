﻿namespace BankSimulation.Application.Interfaces
{
    public interface ICustomException
    {
        public string Title { get; }
        public int StatusCode { get; }
    }
}