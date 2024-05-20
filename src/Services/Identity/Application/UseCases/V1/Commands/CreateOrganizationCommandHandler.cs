﻿using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.UseCases.V1.Commands;

public class CreateOrganizationCommandHandler : ICommandHandler<Command.CreateOrganizationCommand>
{
    private readonly IRepositoryBase<Organization, Guid> _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrganizationCommandHandler(IRepositoryBase<Organization, Guid> organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = Organization.Create(request.Name, request.Industry, request.Description, request.Phone, request.HomePage, request.LogoUrl, request.Street, request.City, request.State, request.Country, request.ZipCode);

        _organizationRepository.Add(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}