﻿using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Auth.Exceptions
{
    public sealed class InvalidRefreshTokenException : CustomException
    {
        public InvalidRefreshTokenException(
            string errorContext,
            string title = "Invalid Refresh Token",
            string details = "The refresh token provided is invalid.")
            : base(title, StatusCodes.Status401Unauthorized, details, errorContext) { }
    }
}