using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.1 Felhasználókezelés - login
/// </summary>
/// <param name="userService"></param>
[ApiController]
public class AuthController(UserService userService): ControllerBase
{
    
    /// <summary>
    /// Belépés
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        var token = await userService.LoginAsync(userDto);
        return Ok(new { Token = token });
    }
}

/// <summary>
/// 3.1 Felhasználókezelés
///
/// A rendszer több felhasználós, így minden felhasználó egyedi azonosítóval, névvel és e-mail
/// címmel rendelkezik. A felhasználók regisztrálhatnak és bejelentkezhetnek, valamint jelszavukat
/// is módosíthatják.
/// </summary>
/// <param name="userService"></param>
[ApiController]
[Route("users")]
public class UserController(UserService userService): ControllerBase
{
    
    /// <summary>
    /// Felhasználó adatainak lekérdezése
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int userId)
    {
        return Ok(await userService.GetUserAsync(userId));
    }
    
    
    /// <summary>
    /// Felhasználó adatainak frissítése
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userCreateDto"></param>
    /// <returns></returns>
    [HttpPut("{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.UpdateUserAsync(userId, userCreateDto);
        return Ok(result);
    }

    
    /// <summary>
    /// Felhasználó törlése
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await userService.DeleteUserAsync(userId);
        return Ok();
    }
    
    
    /// <summary>
    /// Felhasználó regisztráció
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
    {
        var result = await userService.RegisterAsync(userCreateDto);
        return Ok(result);
    }
}