using Application.Repository;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ShowRoomDbContext _db;

        public UserRepository(ShowRoomDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        {
            var query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var pattern = $"%{EscapeLike(search)}%";
                query = query.Where(user =>
                    EF.Functions.ILike(user.first_name, pattern, "\\") ||
                    EF.Functions.ILike(user.last_name, pattern, "\\") ||
                    EF.Functions.ILike(user.email, pattern, "\\"));
            }

            query = query.OrderByDescending(user => user.date_created);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.email == email, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
        }

        private static string EscapeLike(string value)
        {
            return value
                .Replace("\\", "\\\\", StringComparison.Ordinal)
                .Replace("%", "\\%", StringComparison.Ordinal)
                .Replace("_", "\\_", StringComparison.Ordinal);
        }
    }
}
