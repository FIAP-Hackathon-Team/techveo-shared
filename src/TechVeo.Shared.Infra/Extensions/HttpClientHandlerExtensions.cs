using Microsoft.Extensions.DependencyInjection.Extensions;
using TechVeo.Shared.Infra.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientHandlerExtensions
    {
        public static IHttpClientBuilder AddTechVeoClient<TClient, TImplementation>(this IServiceCollection services, string serviceName)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.TryAddTransient<AuthenticatedHttpClientHandler>();

            return services.AddHttpClient<TClient, TImplementation>(
                serviceName,
                (serviceProvider, client) =>
                {
                    var serviceUrlProvider = serviceProvider.GetRequiredService<IServiceUrlProvider>();
                    client.BaseAddress = serviceUrlProvider.GetServiceUri(serviceName);
                })
                .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
        }
    }
}
