using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CryptoSim.Model;
using CryptoSim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CryptoSim.Services.Impl;

public class UserServiceImpl(AppDbContext context, IMapper mapper) : UserService
{
    public async Task<UserDto> RegisterAsync(UserCreateDto userDto)
    {
        var emailAlreadyInUse = await context.Users.AnyAsync(x => x.Email == userDto.Email);
        if (emailAlreadyInUse) 
        {
            throw new BadRequestException("Validation error", "The given email address is already in use.");
        }
        
        var user = mapper.Map<User>(userDto);
        user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return mapper.Map<UserDto>(user);
    }

    public async Task<string> LoginAsync(UserLoginDto userDto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Email == userDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            throw new BadRequestException("Validation error", "Invalid email or password!");
        }

        return await GenerateToken(user);
    }

    public async Task<UserDto> UpdateUserAsync(int userId, UserCreateDto userDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) 
        {
            //FIXME 404
            throw new BadRequestException("Validation error",$"User not found with id: {userId}");
        }
        mapper.Map(userDto, user);

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserAsync(int userId)
    {
        var user = await context.Users
            .FindAsync(userId);
        if (user == null)
        {
            //FIXME 404
            throw new BadRequestException("Validation error",$"User not found with id: {userId}");
        }
        return mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = context.Users
            .FirstOrDefault(x => x.Id == userId);
        if (user == null)
        {
            //FIXME 404
            throw new BadRequestException("Validation error", $"User not found with id: {userId}");
        }
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
    
    private async Task<string> GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("randomSztring12345_x2____randomSztring12345_x2"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(5));

        var id = await GetClaimsIdentity(user);
        var token = new JwtSecurityToken("https://localhost:5177", "https://localhost:5177", id.Claims, expires: expires, signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
        };

        return new ClaimsIdentity(claims, "Token");
    }
}