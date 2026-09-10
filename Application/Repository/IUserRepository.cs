using Domain.Common;
using Domain.Entities;

namespace Application.Repository
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
    }
}
