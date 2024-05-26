namespace Domain.Exceptions;

public static class EmployeeException
{
    public class EmployeeAlreadyExistException : BadRequestException
    {
        public EmployeeAlreadyExistException(string email) 
            : base($"Employee with email {email} already exist.")
        {
        }
    }
}