namespace Domain.Exceptions;

public static class CategoryException
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid categoryId)
            : base($"Category with Id {categoryId} was not found.")
        {
        }
    }
}