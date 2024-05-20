using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors;

public class ConvertDomainEventsToOutboxMessagesInterceptors : SaveChangesInterceptor
{

}