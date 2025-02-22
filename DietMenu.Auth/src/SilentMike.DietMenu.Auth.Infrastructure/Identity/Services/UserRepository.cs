﻿namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Services;

using global::AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Services;
using SilentMike.DietMenu.Auth.Infrastructure.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Data;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal sealed class UserRepository : IUserRepository
{
    private readonly IDietMenuDbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;

    public UserRepository(IDietMenuDbContext context, IMapper mapper, UserManager<User> userManager)
    {
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    public async Task CreateUserAsync(string password, UserEntity user, CancellationToken cancellationToken = default)
    {
        var family = await this.context.Families.SingleAsync(family => family.Id == user.FamilyId, CancellationToken.None);

        var userId = user.Id.ToString();

        var userToCreate = new User
        {
            Email = user.Email,
            Family = family,
            FamilyId = family.Id,
            FamilyKey = family.Key,
            FirstName = user.FirstName,
            Id = userId,
            LastName = user.LastName,
            Role = user.Role,
            UserName = user.Email,
        };

        var result = await this.userManager.CreateAsync(userToCreate, password);

        if (result.Succeeded is false)
        {
            throw new CreateUserException(user.Email, result.Errors.First().Description);
        }
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await this.userManager.FindByEmailAsync(email);

        return this.MapUser(user);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = id.ToString();

        var user = await this.userManager.FindByIdAsync(userId);

        return this.MapUser(user);
    }

    private UserEntity? MapUser(User? user)
    {
        if (user is null)
        {
            return null;
        }

        var result = this.mapper.Map<UserEntity>(user);

        return result;
    }
}
