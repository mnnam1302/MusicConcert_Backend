using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Category;

public class DeleteCategoryCommandHandler : ICommandHandler<Command.DeleteCategoryCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var holderCategory = await _categoryRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new CategoryException.CategoryNotFoundException(request.Id);

        // Persistence into DB
        _categoryRepository.Remove(holderCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}