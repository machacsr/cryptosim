using System.Security.Claims;
using CryptoSim.Dto;
using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.4 Kriptovaluta adás-vétel
/// 
/// A felhasználó a pénztárcájában lévő egyenlegből vásárolhat kriptovalutákat, illetve eladhatja
/// azokat az aktuális piaci árfolyamon.
/// </summary>
[ApiController]
[Route("trade")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TradeController(TradeService tradeService): ControllerBase
{
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    /// <summary>
    /// Kriptovaluta vásárlása
    /// </summary>
    /// <returns></returns>
    [HttpPost("buy/{userId:int}")]
    public async Task<IActionResult> Buy(int? userId, [FromBody] CryptoTradeDto cryptoTradeDto)
    {
        return Ok(await tradeService.BuyCryptoAsync(userId ?? UserId, cryptoTradeDto));
    }
    
    /// <summary>
    /// Kriptovaluta eladása
    /// </summary>
    /// <returns></returns>
    [HttpPost("sell/{userId:int}")]
    public async Task<IActionResult> Sell(int? userId, CryptoTradeDto cryptoTradeDto)
    {
        return Ok(await tradeService.SellCryptoAsync(userId ?? UserId, cryptoTradeDto));
    }
}