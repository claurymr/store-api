using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Contracts;

namespace Store.Api.Services;
public class AuthService(AuthSettings authSettings) : IAuthService
{
    private readonly AuthSettings _authSettings = authSettings;

    public async Task<TokenResponse> GenerateToken(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_authSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, _authSettings.Role)
                ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _authSettings.Issuer,
            Audience = _authSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return await Task.FromResult(TokenResponse.Success(tokenString));
    }
}