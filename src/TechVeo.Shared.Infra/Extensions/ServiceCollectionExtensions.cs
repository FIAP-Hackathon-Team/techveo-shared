using System;
using System.Linq;
using Amazon.Runtime;
using Amazon.S3;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TechVeo.Shared.Application.Events;
using TechVeo.Shared.Application.Http;
using TechVeo.Shared.Application.Storage;
using TechVeo.Shared.Domain.Events;
using TechVeo.Shared.Domain.UoW;
using TechVeo.Shared.Infra.Events;
using TechVeo.Shared.Infra.Extensions;
using TechVeo.Shared.Infra.Http;
using TechVeo.Shared.Infra.Persistence.Contexts;
using TechVeo.Shared.Infra.Persistence.UoW;
using TechVeo.Shared.Infra.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfra<DbContext>(this IServiceCollection services, InfraOptions? options = null)
         where DbContext : TechVeoContext
    {
        //Context
        services.TryAddScoped<DbContext>();
        services.AddDbContext<DbContext>((sp, dbOptions) => options?.DbContext?.Invoke(sp, dbOptions));

        //UoW
        services.TryAddScoped<IUnitOfWorkTransaction, UnitOfWorkTransaction>();
        services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DbContext>());

        //DomainEvents
        services.TryAddScoped<IDomainEventStore>(sp => sp.GetRequiredService<DbContext>());

        return services.AddSharedInfra(options);
    }

    public static IServiceCollection AddSharedInfra(this IServiceCollection services, InfraOptions? options = null)
    {
        options ??= new InfraOptions();

        services.TryAddSingleton(Options.Options.Create(options));

        //MediatR
        services.AddMediatR(options.ApplicationAssembly);

        var mediatR = services.First(s => s.ServiceType == typeof(IMediator));

        services.Replace(ServiceDescriptor.Transient<IMediator, TechVeo.Shared.Infra.EventualConsistency.Mediator>());
        services.Add(
            new ServiceDescriptor(
                mediatR.ServiceType,
                TechVeo.Shared.Infra.EventualConsistency.Mediator.ServiceKey,
                mediatR.ImplementationType!,
                mediatR.Lifetime));

        //EventBus
        services.TryAddSingleton<IEventBus, RabbitMqEventBus>();

        //ServiceUrlProvider
        services.TryAddSingleton<IServiceUrlProvider, ServiceUrlProvider>();

        //TokenService
        services.AddMemoryCache();
        services.AddHttpClient<ITokenService, TokenService>((sp, client) =>
        {
            var serviceUrlProvider = sp.GetRequiredService<IServiceUrlProvider>();
            client.BaseAddress = serviceUrlProvider.GetServiceUri("Authentication");
        });

        //Storage
        services.AddOptions<StorageOptions>().BindConfiguration(StorageOptions.SectionName);

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var storageOptions = sp.GetRequiredService<IOptions<StorageOptions>>().Value;

            if (storageOptions.S3 == null)
            {
                throw new InvalidOperationException("Storage:S3 configuration section is missing. Please configure Storage:S3 in appsettings.json");
            }

            var s3Options = storageOptions.S3;

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(s3Options.Region)
            };

            if (!string.IsNullOrEmpty(s3Options.ServiceUrl))
            {
                s3Config.ServiceURL = s3Options.ServiceUrl;
                s3Config.ForcePathStyle = s3Options.ForcePathStyle;
            }

            if (!string.IsNullOrEmpty(s3Options.AccessKey) && !string.IsNullOrEmpty(s3Options.SecretKey))
            {
                var credentials = new BasicAWSCredentials(s3Options.AccessKey, s3Options.SecretKey);
                return new AmazonS3Client(credentials, s3Config);
            }

            return new AmazonS3Client(s3Config);
        });

        services.AddScoped<IStorageService, S3StorageService>();
        services.AddScoped<VideoStorageHelper>();

        return services;
    }
}
