using CryptoSim.Dto;
using CryptoSim.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.5 Árfolyam változás kezelése
/// 
/// Manuális árfolyam frissítés
/// </summary>
[ApiController]
[Route("crypto/price")]//based on requirements
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExchangeRateController(ExchangeRateService exchangeRateService): ControllerBase
{
    /// <summary>
    /// Manuális árfolyam frissítés
    /// </summary>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCryptoListingAsync([FromBody] CreateCryptoListingDto cryptoListingDto)
    {
        var result = await exchangeRateService.CreateCryptoListingAsync(cryptoListingDto);
        return Ok(result);
    }
    
    /// <summary>
    /// Árfolyamváltozási naplózás
    /// </summary>
    [HttpGet("history/{cryptoId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCryptoListingAsync(int cryptoId)
    {
        var result = await exchangeRateService.GetListingsForCrypto(cryptoId);
        return Ok(result);
    }
}