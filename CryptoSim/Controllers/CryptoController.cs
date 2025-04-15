using CryptoSim.Dto;
using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.3 Kriptovaluták kezelése
/// 
/// A rendszer 15 féle kriptovalutát kezel, melyeknek előre definiált kezdő árfolyamuk és
/// mennyiségük van. Az árfolyamok folyamatosan változnak, és minden kriptovalutának külön
/// azonosítója van.
/// </summary>
[ApiController]
[Route("cryptos")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CryptoController(CryptoService cryptoService): ControllerBase
{
    /// <summary>
    /// Kriptovaluták listázása
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCryptos()
    {
        return Ok(await cryptoService.GetAllCryptoAsync());
    }
    
    /// <summary>
    /// Egy adott kriptovaluta lekérdezése 
    /// </summary>
    /// <returns></returns>
    [HttpGet("{cryptoId:int}")]
    public async Task<IActionResult> GetAllCryptos(int cryptoId)
    {
        return Ok(await cryptoService.GetCryptoAsync(cryptoId));
    }
    
    
    /// <summary>
    /// Új kriptovaluta hozzáadása
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateCryptoAsync([FromBody] CryptoDto createRequestDto)
    {
        return Ok(await cryptoService.CreateCryptoAsync(createRequestDto));
    }
    
    /// <summary>
    /// Kriptovaluta törlése
    /// </summary>
    /// <returns></returns>
    [HttpPost("{cryptoId:int}")]
    public async Task<IActionResult> DeleteCryptoAsync(int cryptoId)
    {
        await cryptoService.DeleteCryptoAsync(cryptoId);
        return Ok();
    }
}