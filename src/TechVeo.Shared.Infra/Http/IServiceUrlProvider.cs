using System;

namespace TechVeo.Shared.Infra.Http;

public interface IServiceUrlProvider
{
    string GetServiceUrl(string serviceName);

    Uri GetServiceUri(string serviceName);
}
