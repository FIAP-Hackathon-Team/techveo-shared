using System.Threading;
using System.Threading.Tasks;

namespace TechVeo.Shared.Application.Http
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}
