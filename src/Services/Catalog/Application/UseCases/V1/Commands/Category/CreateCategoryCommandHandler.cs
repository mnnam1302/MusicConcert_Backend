using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Commands.Category;

public class CreateCategoryCommandHandler : ICommandHandler<Command.CreateCategoryCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result> Handle(Command.CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Domain.Entities.Category.Create(Guid.NewGuid(), request.Name, request.Description);

        // Persistence into DB
        _categoryRepository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}