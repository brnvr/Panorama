using Microsoft.IdentityModel.Tokens;
using PanoramaApi.Models.Entities;
using PanoramaApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PanoramaApi.Services
{
    public class AuthenticationService
    {
        private AppDbContext _dbContext { get; set; }

        public AuthenticationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Login(string email, string password, params string[] roles)
        {
            User user;
            Role role;

            try
            {
                user = new UserRepository(_dbContext).FindByUsernameOrEmail(email);
                role = new RoleRepository(_dbContext).FindByUserId(user.Id);

                if (roles.Length > 0 && !roles.Contains(role.Name))
                {
                    throw new AuthenticationException("Not allowed.");
                }
            }
            catch (EntityNotFoundException ex)
            {
                throw new AuthenticationException("Incorrect username/e-mail or password.", ex);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new AuthenticationException("Incorrect username/e-mail or password.");
            }

            return GenerateJwt(user, role);
        }

        protected static string GenerateHash()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        protected string GenerateJwt(User user, Role role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, GenerateHash()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim("picturePath", user.PicturePath ?? ""),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };

            var token = new JwtSecurityToken(
                AppSettings.JwtIssuer,
                AppSettings.JwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
