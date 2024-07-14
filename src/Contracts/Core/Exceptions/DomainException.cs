namespace Contracts.Core.Exceptions;

public class DomainException : Exception
{
    public DomainException(string title, string message)
        : base(message)
    {
        Title = title;
    }

    public string Title { get; set; }
}