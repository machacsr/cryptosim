using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.7 Tranzakciók Naplózása
/// 
/// Minden vásárlás és eladás tranzakció naplózásra kerül, hogy a felhasználók visszanézhessék
/// korábbi kereskedéseiket.
/// </summary>
[ApiController]
[Route("transactions")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TransactionController(IUnitOfWork unitOfWork, IMapper mapper): ControllerBase
{
    /// <summary>
    /// Tranzakciók listázása
    /// </summary>
    [HttpGet("{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> TransactionListing(int userId)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, null, ["CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation exception", "User not found");
        }

        return Ok(user.CryptoTransactions.OrderByDescending(it => it.Timestamp).Select(mapper.Map<CryptoTransactionDto>));
    }
    
    
    /// <summary>
    /// Tranzakció részleteinek lekérdezése
    /// </summary>
    [HttpGet("details/{transactionId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> TransactionDetails(string transactionId)
    {
        var cryptoTransaction = await unitOfWork.CryptoTransactionRepository.GetByIdAsync(Guid.Parse(transactionId));
        if (cryptoTransaction == null)
        {
            throw new BadRequestException("Validation exception", "User not found");
        }
        return Ok(mapper.Map<CryptoTransactionDto>(cryptoTransaction));
    }
}