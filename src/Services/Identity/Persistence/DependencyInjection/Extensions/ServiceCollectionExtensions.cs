using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.DependencyInjection.Options;
using Persistence.Interceptors;
using Persistence.Respositories;

namespace Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var auditableInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
            var outboxInterceptors = provider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptors>();

            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            builder
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseLazyLoadingProxies(true)
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString("ConnectionStrings"),
                    sqlServerOptionsAction: optionsBuilder 
                        => optionsBuilder
                        .ExecutionStrategy(
                            dependencies => new SqlServerRetryingExecutionStrategy(
                                dependencies: dependencies,
                                maxRetryCount: options.CurrentValue.MaxRetryCount,
                                maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                errorNumbersToAdd: options.CurrentValue.ErrorNumbersoAdd))
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .AddInterceptors(
                    auditableInterceptor,
                    outboxInterceptors);
        });

        services.AddIdentityCore<AppEmployee>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityCore<AppCustomer>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        return services;
    }

    public static void AddInterceptorPersistence(this IServiceCollection services)
    {
        services.AddTransient<UpdateAuditableEntitiesInterceptor>();
        services.AddTransient<ConvertDomainEventsToOutboxMessagesInterceptors>();
    }

    public static void AddRepositoryPersistence(this IServiceCollection services)
    {
        services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptionsPersistence(this IServiceCollection services, IConfiguration section)
    {
        return services
            .AddOptions<SqlServerRetryOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}