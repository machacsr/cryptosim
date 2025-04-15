using System.Security.Claims;
using CryptoSim.Dto;
using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

[ApiController]
[Route("wallet")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WalletController(WalletService walletService): ControllerBase
{
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetWallet(int? userId)
    {
        return Ok(await walletService.GetWalletAsync(userId ?? UserId));
    }
    
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateBalance(int? userId, [FromBody] WalletBalanceDto balanceDto)
    {
        return Ok(await walletService.UpdateWalletAsync(userId ?? UserId, balanceDto));
    }
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteWallet(int? userId)
    {
        await walletService.DeleteWalletAsync(userId ?? UserId);
        return Ok();
    }
}