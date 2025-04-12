using CryptoSim.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

public class AuthController(IUserService userService): ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        var token = await userService.LoginAsync(userDto);
        return Ok(new { Token = token });
    }
}

[ApiController]
[Route("users")]
public class UserController(IUserService userService): ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(int userId)
    {
        return Ok(await userService.GetUserAsync(userId));
    }
    
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.UpdateUserAsync(userId, userCreateDto);
        return Ok(result);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await userService.DeleteUserAsync(userId);
        return Ok();
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.RegisterAsync(userCreateDto);
        return Ok(result);
    }
}