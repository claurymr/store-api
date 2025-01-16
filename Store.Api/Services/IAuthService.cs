using Store.Api.Contracts;

namespace Store.Api.Services;
public interface IAuthService
{
    Task<TokenResponse> GenerateToken(string userName);
}