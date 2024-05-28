namespace Domain.Exceptions;

public static class EventException
{
    public class EventNotFoundException : NotFoundException
    {
        public EventNotFoundException(Guid eventId)
            : base($"Event with Id {eventId} was not found.")
        {
        }
    }

    public class EventFieldException : BadRequestException
    {
        public EventFieldException(string fieldName)
            : base($"Event with field {fieldName} is not correct.")
        {
        }
    }

    public class EventTypeException : BadRequestException
    {
        public EventTypeException(string eventType)
            : base($"Event with corresponding EventType {eventType} is not correct.")
        {
        }
    }
}