namespace PromerceCRM.API.Identity
{
    public interface IAuthService
    {
        Task<(bool, string)> Register(RegisterModel model);
        Task<UserDto> LoginAsync(LoginModel model);
        Task UpdateUserAsync(UserDto userObj);
    }
}
