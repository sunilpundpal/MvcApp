using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MvcAppJwtAuth.Models;

namespace MvcAppJwtAuth.Services
{

    public class TokenService : ITokenService
    {
        private readonly string _encryptionKey;
        private readonly string issuer;
        public const int EXPIRY_DURATION_MINUTES = 30;

        public TokenService(string encryptionKey, string issuer)
        {
            _encryptionKey = encryptionKey;
            this.issuer = issuer;
        }

        public string BuildToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_encryptionKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsValidToken(string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(_encryptionKey);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}