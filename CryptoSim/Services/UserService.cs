namespace CryptoSim.Services;

public interface UserService
{
    Task<UserDto> RegisterAsync(UserCreateDto userDto);
    Task<string> LoginAsync(UserLoginDto userDto);
    Task<UserDto> UpdateUserAsync(int userId, UserCreateDto userDto);
    Task<UserDto> GetUserAsync(int userId);
    Task DeleteUserAsync(int userId);
}