using System;

namespace TechVeo.Shared.Application.Http;

public interface IServiceUrlProvider
{
    string GetServiceUrl(string serviceName);

    Uri GetServiceUri(string serviceName);
}
