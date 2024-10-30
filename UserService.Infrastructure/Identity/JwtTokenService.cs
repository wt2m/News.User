using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Identity
{
    internal class JwtTokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;
        private readonly string _audience;
        private readonly string _issuer;

        public JwtTokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
            _audience = _configuration["Jwt:Audience"]!;
            _issuer = _configuration["Jwt:Issuer"]!;

        }

        public async Task<string> GenerateTokenAsync(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var appUser = await _userManager.FindByIdAsync(userId.ToString());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser!.UserName!),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(appUser);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _audience,
                Issuer = _issuer,
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public bool VerifyTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal? principal = null;

            try
            {
                principal  = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Disable clock skew
                }, out SecurityToken validatedToken);
            }
            catch
            {
            }
            
            return principal != null;
        }

        public Guid GetUserIdByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal? principal = null;

            try
            {
                principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Disable clock skew
                }, out SecurityToken validatedToken);

                var userId = new Guid(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

                return userId;
            }
            catch
            {
                throw new Exception("Error trying to retrieve user id from token");
            }

        }
    }
}
