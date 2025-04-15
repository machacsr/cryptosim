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
[Route("portfolio")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PortfolioController(PortfolioService portfolioService): ControllerBase
{
        
    /// <summary>
    /// Felhasználó portfóliójának lekérdezése
    /// </summary>
    /// <returns></returns>
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserPortfolio(int userId)
    {
        return Ok(await portfolioService.GetPortfolio(userId));
    }
}