using TechVeo.Shared.Worker.Events;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorker(this IServiceCollection services)
    {
        //EventBus
        services.AddHostedService<EventBusWorker>();

        return services;
    }
}
