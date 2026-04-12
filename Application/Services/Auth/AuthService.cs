using Application.Persistence;
using Application.Services.Auth.Exceptions;
using Application.Services.Auth.Request.Login;
using Application.Services.Auth.Request.Register;
using Domain.Identity;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserEntity> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException("Invalid email or password.");
        }

        if (!Argon2.Verify(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException("Invalid email or password.");
        }

        return user;
    }

    public async Task<UserEntity> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _dbContext.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);
            
        if (existingUser)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        var defaultRole = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);

        var user = new UserEntity
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = Argon2.Hash(request.Password),
        };

        if (defaultRole != null)
        {
            user.Roles.Add(defaultRole);
        }

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}
