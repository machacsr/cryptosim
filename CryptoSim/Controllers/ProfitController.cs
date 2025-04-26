using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;
using CryptoSim.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Controllers;

/// <summary>
/// 3.6 Nyereség/Veszteség Számítása
/// 
/// A felhasználók nyomon követhetik portfóliójuk értékének változását az árfolyamok alapján.
/// </summary>
[ApiController]
[Route("profit")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfitController(IUnitOfWork unitOfWork): ControllerBase
{
    
    /// <summary>
    /// Profit/Veszteség kiszámítása egy adott pillanatban
    /// </summary>
    [HttpGet("{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> TotalDelta(int userId)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, null, ["CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation exception", "User not found");
        }

        var latestListingGroupByCrypto = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.State == CryptoListingState.Active))
            .GroupBy(listing => listing.CryptoId)
            .ToDictionary(g => g.Key, g => g.First());
        
        // SUM( ACTUAL_PRICE - PURCHASE_PRICE )
        var response = new ProfitDto()
        {
            TotalDelta = user.CryptoTransactions.ToList()
                .Where(transaction => transaction.TransactionType == CryptoTransactionType.Buy)
                .Sum(transaction => latestListingGroupByCrypto.GetValueOrDefault(transaction.CryptoId)!.Price - transaction.TotalAmount)
        };
        
        return Ok(response);
    }
    
    
    /// <summary>
    /// Profit/Veszteség részletes lebontásban 
    /// </summary>
    [HttpGet("details/{userId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeltasByCrypto(int userId)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, null, ["CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation exception", "User not found");
        }

        var response = new ProfitDetailsDto();

        var transactionsGroupByCrypto = user.CryptoTransactions.GroupBy(t => t.CryptoId).ToDictionary(g => g.Key, g => g.ToList());
        
        var latestListingGroupByCrypto = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.State == CryptoListingState.Active))
            .GroupBy(listing => listing.CryptoId)
            .ToDictionary(g => g.Key, g => g.First());
        
        foreach (var transactionGroup in transactionsGroupByCrypto)
        {
            var crypto = await unitOfWork.CryptoRepository.GetByIdAsync(transactionGroup.Key);
            if (crypto == null) continue; // not exists case
            var holdedCryptoItem = TransactionEvaluator.EvaluateByCrypto(transactionGroup.Value, crypto);

            // SUM( ACTUAL_PRICE - PURCHASE_PRICE )
            var aggregatedProfit = transactionGroup.Value.ToList()
                .Where(transaction => transaction.TransactionType == CryptoTransactionType.Buy)
                .Sum(transaction => latestListingGroupByCrypto.GetValueOrDefault(crypto.Id)!.Price - transaction.TotalAmount);
            
            response.CryptoDeltas.Add(new CryptoProfitDetail()
            {
                CryptoId = holdedCryptoItem.CryptoId,
                Name = holdedCryptoItem.Name,
                Symbol = holdedCryptoItem.Symbol,
                Quantity = holdedCryptoItem.Quantity,
                MarketPrice = latestListingGroupByCrypto.GetValueOrDefault(crypto.Id)!.Price,
                Delta = aggregatedProfit,
            });
        }
        
        response.TotalDelta = response.CryptoDeltas.Sum(delta => delta.Delta);

        return Ok(response);
    }
    
}