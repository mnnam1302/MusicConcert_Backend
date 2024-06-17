using Application.Abstractions;
using Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions jwtOptions = new JwtOptions();

    public JwtTokenService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var screteKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
        var signatureCredentials = new SigningCredentials(screteKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOptions.ExpireMin), // 15 mins
            signingCredentials: signatureCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[128];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
            var refreshToken = Convert.ToBase64String(bytes);
            return refreshToken;
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var Key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, // Set to true if you want to validate the Audience
            ValidateIssuer = false, // Set to true if you want to validate the Issuer
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero // No clock skew for simplicity (adjust as needed)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}