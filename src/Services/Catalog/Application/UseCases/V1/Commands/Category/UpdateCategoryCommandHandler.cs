using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Category;

public class UpdateCategoryCommandHandler : ICommandHandler<Command.UpdateCategoryCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var holderCategory = await _categoryRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new CategoryException.CategoryNotFoundException(request.Id);


        // DDD
        holderCategory.Update(request.Name, request.Description);

        // Persistence into DB
        _categoryRepository.Update(holderCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}