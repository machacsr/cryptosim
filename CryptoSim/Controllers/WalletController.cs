using System.Security.Claims;
using CryptoSim.Dto;
using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;


/// <summary>
/// 3.2 Felhasználó Pénztárcája
/// 
/// A rendszer indításakor minden felhasználó kap egy virtuális pénztárcát egy kezdeti egyenleggel. A
/// pénztárca tartalmazza a felhasználó egyenlegét és az általa birtokolt kriptovalutákat.
/// </summary>
/// <param name="walletService"></param>
[ApiController]
[Route("wallet/{userId:int}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WalletController(WalletService walletService): ControllerBase
{
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
    /// <summary>
    /// Pénztárca lekérdezése
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetWallet(int? userId)
    {
        return Ok(await walletService.GetWalletAsync(userId ?? UserId));
    }
    
    /// <summary>
    /// Pénztárca egyenleg frissítése
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="balanceDto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateBalance(int? userId, [FromBody] WalletBalanceDto balanceDto)
    {
        return Ok(await walletService.UpdateWalletAsync(userId ?? UserId, balanceDto));
    }
    
    /// <summary>
    /// Pénztárca törlése
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteWallet(int? userId)
    {
        await walletService.DeleteWalletAsync(userId ?? UserId);
        return Ok();
    }
}