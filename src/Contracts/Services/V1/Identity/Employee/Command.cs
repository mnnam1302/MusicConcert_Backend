﻿using Contracts.Abstractions.Message;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Command
{
    public record CreateEmployeeCommand(
        string FirstName, 
        string LastName, 
        string PhoneNumber, 
        DateTime? DateOfBirth,
        string Email,
        string Password,
        string PasswordConfirmation,
        Guid? OrganizationId) : ICommand;

    public record UpdateEmployeeCommand(
        Guid Id, 
        string FirstName, 
        string LastName, 
        DateTimeOffset? DateTimeOffset,
        Guid? OrganizationId) : ICommand;

    public record DeleteEmployeeCommand(Guid Id) : ICommand;


    // Identity
    public record LogoutEmployeeCommand(string Email) : ICommand;
}