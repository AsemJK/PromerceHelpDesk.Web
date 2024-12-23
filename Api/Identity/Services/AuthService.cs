using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PromerceCRM.API.Identity
{
    public class AuthService : IAuthService
    {
        public AuthService(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public UserManager<UserModel> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public IConfiguration _configuration { get; }

        public async Task<UserDto> LoginAsync(LoginModel model)
        {
            UserDto userDto = new UserDto();
            var user = await _userManager.FindByNameAsync(model.UserName);
            userDto.LoginResult = "Ok";

            if (user == null) userDto.LoginResult = "Invalid_Username";
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                userDto = new UserDto();
                userDto.LoginResult = "Invalid_Password";
                goto exit;
            }
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                user.TenantCode = model.TenantCode = "ADMIN";

            if (model.TenantCode != user.TenantCode) userDto.LoginResult = "Invalid_Tenant";
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            List<string> roles = new List<string>();
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                roles.Add(userRole);
            }
            string token = GenerateToken(authClaims);
            userDto.Token = token;
            userDto.UserName = user.UserName;
            userDto.Id = user.Id;
            userDto.TenantCode = user.TenantCode;
            userDto.IsActive = user.IsActive;
            userDto.Roles = roles;
        exit:
            return userDto;
        }

        public async Task<(bool, string)> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return (false, "User already exists");

            UserModel user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                TenantCode = model.TenantCode
            };
            try
            {
                var createUserResult = await _userManager.CreateAsync(user, model.Password);
                if (!createUserResult.Succeeded)
                    return (false, "User creation failed! Please check user details and try again.");

                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));

                if (await _roleManager.RoleExistsAsync(model.Role))
                    await _userManager.AddToRoleAsync(user, model.Role);

                return (true, "User created successfully!");
            }
            catch (Exception ex)
            {
                return (false, "Failed to create the user !");
            }

        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Secret"]));
            var _TokenExpiryTimeInMinutes = Convert.ToInt64(_configuration["JwtOptions:TokenExpiryTimeInMinutes"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtOptions:ValidIssuer"],
                Audience = _configuration["JwtOptions:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task UpdateUserAsync(UserDto userObj)
        {
            var user = _userManager.FindByIdAsync(userObj.Id).Result;
            if (user != null)
            {
                user.IsActive = userObj.IsActive;
            }

            await _userManager.UpdateAsync(user);
        }
    }
}
