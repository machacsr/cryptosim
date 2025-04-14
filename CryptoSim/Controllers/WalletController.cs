using System.Security.Claims;
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
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await walletService.GetWalletAsync(userId));
    }
}