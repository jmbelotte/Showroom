using Domain.Common;
using Domain.Entities;

namespace Application.Services
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetPagedAsync(int page, string? search = null, CancellationToken cancellationToken = default);
        Task<User> CreateAsync(string firstName, string lastName, string email, CancellationToken cancellationToken = default);
    }
}
