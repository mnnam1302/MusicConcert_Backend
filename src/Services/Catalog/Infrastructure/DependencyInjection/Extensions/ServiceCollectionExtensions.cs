using Application.Abstractions;
using Contracts.JsonConverters;
using Firebase.Auth;
using Firebase.Storage;
using Infrastructure.BackgroundJobs;
using Infrastructure.DependencyInjection.Options;
using Infrastructure.PipelineObservers;
using Infrastructure.UploadImage;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using System.Reflection;

namespace Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFirebaseInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var fireBaseOptions = new FireBaseOptions();
        configuration.GetSection(nameof(FireBaseOptions)).Bind(fireBaseOptions);

        // Firebase config
        services.AddTransient(provider 
            => new FirebaseConfig(fireBaseOptions.ApiKey));

        // Firebase storage
        services.AddTransient(provider 
            => new FirebaseStorage(fireBaseOptions.StorageBucket));

        // Firebase AuthProvider
        services.AddTransient<FirebaseAuthProvider>();

        services.AddTransient<IFirebaseService, FirebaseService>();

        return services;
    }

    public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var massTransitConfiguration = new MasstransitConfiguration();
        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(massTransitConfiguration);

        var messageBusOptions = new MessageBusOptions();
        configuration.GetSection(nameof(MessageBusOptions)).Bind(messageBusOptions);

        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumers(Assembly.GetExecutingAssembly());

            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, bus) =>
            {
                bus.Host(massTransitConfiguration.Host, massTransitConfiguration.Port, massTransitConfiguration.VHost, h =>
                {
                    h.Username(massTransitConfiguration.Username);
                    h.Password(massTransitConfiguration.Password);
                });

                bus.UseMessageRetry(retry
                    => retry.Incremental(
                        retryLimit: messageBusOptions.RetryLimit,
                        initialInterval: messageBusOptions.InitialInterval,
                        intervalIncrement: messageBusOptions.IntervalIncrement));

                bus.UseNewtonsoftJsonSerializer();

                bus.ConfigureNewtonsoftJsonSerializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());

                    return settings;
                });

                bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());

                    return settings;
                });

                bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                bus.ConnectSendObserver(new LoggingSendObserver());
                bus.ConnectPublishObserver(new LoggingPublishObserver());

                bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                bus.ConfigureEventReceiveEndpoints(context);
                bus.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static void AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProducerOutboxMessageJob));
            configure
                .AddJob<ProducerOutboxMessageJob>(jobKey)
                .AddTrigger(trigger
                    => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                        {
                            schedule.WithInterval(TimeSpan.FromMilliseconds(100));
                            schedule.RepeatForever();
                        }));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();
    }
}