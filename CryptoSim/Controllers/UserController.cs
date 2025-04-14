using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

[ApiController]
public class AuthController(UserService userService): ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        var token = await userService.LoginAsync(userDto);
        return Ok(new { Token = token });
    }
}

[ApiController]
[Route("users")]
public class UserController(UserService userService): ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get(int userId)
    {
        return Ok(await userService.GetUserAsync(userId));
    }
    
    [HttpPut("{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.UpdateUserAsync(userId, userCreateDto);
        return Ok(result);
    }

    [HttpDelete("{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await userService.DeleteUserAsync(userId);
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.RegisterAsync(userCreateDto);
        return Ok(result);
    }
}