using Application.Repository;
using Domain.Common;
using Domain.Entities;

namespace ShowRoom.Tests.Fakes;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = _users
            .OrderByDescending(user => user.date_created)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<User>
        {
            Items = items,
            TotalCount = _users.Count,
            Page = page,
            PageSize = pageSize
        });
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(item => item.email == email);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }
}
