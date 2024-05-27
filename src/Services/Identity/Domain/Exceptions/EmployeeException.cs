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

    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId)
            : base($"Employee with Id {employeeId} was not found.")
        {
        }
    }

    public class EmployeeNotFoundByEmailException : NotFoundException
    {
        public EmployeeNotFoundByEmailException(string email) 
            : base($"Employee with Email {email} was not found.")
        {
        }
    }

    public class EmployeeFieldException : BadRequestException
    {
        public EmployeeFieldException(string fieldName)
            : base($"Employee with field {fieldName} is not correct.")
        {
        }
    }
}