using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using elastic_app.domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using elastic_app.domain.Abstractions;

namespace elastic_app.infrastructure.Security
{
    internal sealed class StatelessTokenProvider(IConfiguration configuration) : ITokenProvider
    {
        public string Create(UserModel userModel)
        {
            string secretKey = configuration["Jwt:Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
                        new Claim("email_verified", userModel.EmailVerified.ToString())
                    ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
