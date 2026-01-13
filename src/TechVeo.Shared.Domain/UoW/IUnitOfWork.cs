using System.Threading.Tasks;

namespace TechVeo.Shared.Domain.UoW;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();

    Task RollbackAsync();
}
