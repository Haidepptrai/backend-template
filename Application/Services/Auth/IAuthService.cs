using Application.Services.Auth.Request.Login;
using Application.Services.Auth.Request.Register;
using Domain.Identity;

namespace Application.Services.Auth;

public interface IAuthService
{
    Task<UserEntity> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<UserEntity> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
