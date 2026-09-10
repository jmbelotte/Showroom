using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using ShowRoom.Tests.Fakes;

namespace ShowRoom.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenFirstNameIsMissing_ThrowsValidationException()
    {
        var service = new UserService(new InMemoryUserRepository());

        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => service.CreateAsync(" ", "Lovelace", "ada@showroom.local"));

        Assert.True(exception.Errors.ContainsKey("first_name"));
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAlreadyExists_ThrowsDuplicateEmailException()
    {
        var service = new UserService(new InMemoryUserRepository());
        await service.CreateAsync("Ada", "Lovelace", "ada@showroom.local");

        var exception = await Assert.ThrowsAsync<DuplicateEmailException>(
            () => service.CreateAsync("Ada", "Lovelace", "ADA@showroom.local"));

        Assert.Equal("ada@showroom.local", exception.Email);
    }

    [Fact]
    public async Task GetPagedAsync_WhenMoreThanPageSize_ReturnsTwentyItemsOnFirstPage()
    {
        var repository = new InMemoryUserRepository();
        var service = new UserService(repository);

        for (var i = 1; i <= 25; i++)
        {
            await repository.AddAsync(new User
            {
                id = Guid.NewGuid(),
                first_name = $"User{i}",
                last_name = "Demo",
                email = $"user{i}@showroom.local",
                date_created = DateTime.UtcNow.AddMinutes(-i)
            });
        }

        var page1 = await service.GetPagedAsync(1);
        var page2 = await service.GetPagedAsync(2);

        Assert.Equal(20, page1.Items.Count);
        Assert.Equal(25, page1.TotalCount);
        Assert.Equal(UserService.PageSize, page1.PageSize);
        Assert.Equal(2, page1.TotalPages);
        Assert.Equal(5, page2.Items.Count);
    }
}
