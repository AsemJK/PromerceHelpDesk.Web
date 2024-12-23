using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PromerceCRM.API.Identity;
using PromerceCRM.API.Identity.Models.DTOs;
using PromerceCRM.API.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace PromerceHelpDesk.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        public AuthenticationController(IAuthService authService,
            ILogger<AuthenticationController> logger,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            ITenantService tenantService)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _tenantService = tenantService;
        }

        public IAuthService _authService { get; }
        public ILogger<AuthenticationController> _logger { get; }
        public UserManager<UserModel> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public ITenantService _tenantService { get; }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var userLogin = await _authService.LoginAsync(user);

                if (userLogin.LoginResult == "Ok")
                {
                    HttpContext.Session.SetString("user", JsonSerializer.Serialize(userLogin));
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    // Add other claims as needed
                };
                    foreach (var userRole in userLogin.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Configure auth properties as needed
                    };

                    // Sign the user in
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    if (User.Identity.IsAuthenticated)
                    {
                        // User is authenticated
                    }
                    else
                    {
                        // User is not authenticated
                    }
                }

                return Ok(userLogin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _authService.Register(user);
                if (!status)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        /*
        [HttpGet("Roles")]
        [Authorize]
        public async Task<List<IdentityRole>> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }
        */

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto obj)
        {
            var user = _userManager.FindByIdAsync(obj.UserId).Result;
            if (user != null)
            {
                if (_userManager.CheckPasswordAsync(user, obj.CurrentPassword).Result)
                {
                    //current Password is correct
                    if (obj.NewPassword == obj.ConfirmPassword)
                    {
                        await _userManager.ChangePasswordAsync(user, obj.CurrentPassword, obj.NewPassword);
                        return Ok("Done");
                    }
                    else
                        return Ok("Password and Confirm not matched");
                }
                else
                    return Ok("Invalid Password");
            }
            else
                return Ok("Invalid User");
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<List<UserDto>> Users()
        {
            List<UserDto> usersList = new List<UserDto>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                List<string> rolesList = userRoles.ToList();
                usersList.Add(new UserDto
                {
                    Id = user.Id,
                    LoginResult = "-",
                    Password = "-",
                    Roles = rolesList,
                    TenantCode = user.TenantCode,
                    TenantName = _tenantService.GetByCode(user.TenantCode)?.TenantName,
                    Token = "-",
                    UserName = user.UserName
                });
            }

            return usersList;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Update(UserDto user)
        {
            try
            {
                await _authService.UpdateUserAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
