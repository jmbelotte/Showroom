using Application.Exceptions;
using Application.Repository;
using Domain.Common;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public const int PageSize = 20;

        private readonly IUserRepository _users;

        public UserService(IUserRepository users)
        {
            _users = users;
        }

        public Task<PagedResult<User>> GetPagedAsync(int page, string? search = null, CancellationToken cancellationToken = default)
        {
            if (page < 1)
            {
                page = 1;
            }

            var term = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
            return _users.GetPagedAsync(page, PageSize, term, cancellationToken);
        }

        public async Task<User> CreateAsync(string firstName, string lastName, string email, CancellationToken cancellationToken = default)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                errors["first_name"] = ["First name is required."];
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                errors["last_name"] = ["Last name is required."];
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                errors["email"] = ["A valid email is required."];
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();
            var existing = await _users.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (existing is not null)
            {
                throw new DuplicateEmailException(normalizedEmail);
            }

            var user = new User
            {
                id = Guid.NewGuid(),
                first_name = firstName.Trim(),
                last_name = lastName.Trim(),
                email = normalizedEmail,
                date_created = DateTime.UtcNow
            };

            await _users.AddAsync(user, cancellationToken);
            return user;
        }
    }
}
