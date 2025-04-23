using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;

namespace CryptoSim.Services.Impl;

public class PortfolioServiceImpl(IUnitOfWork unitOfWork, IMapper mapper, WalletService walletService): PortfolioService
{
    public async Task<PortfolioDto> GetPortfolio(int userId)
    {
        // get wallet for user
        var walletDto = await walletService.GetWalletAsync(userId);
        
        // find actual crypto listings
        var latestCryptoListings = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.State == CryptoListingState.Active, ["Crypto"])).ToList();

        // initiate portfolio
        var portfolio = new PortfolioDto()
        {
            UserId = userId
        };
        
        foreach (var walletCryptoItem in walletDto.WalletCryptoItems)
        {
            // search market price for wallet item
            var listingForWalletItem = latestCryptoListings.First(listing => listing.Crypto.Name == walletCryptoItem.Name && listing.Crypto.Symbol == walletCryptoItem.Symbol);

            // portfolio item
            portfolio.PortfolioItems.Add(new PortfolioItemDto()
            {
                Name = walletCryptoItem.Name,
                CryptoId = walletCryptoItem.CryptoId,
                Symbol = walletCryptoItem.Symbol,
                Quantity = walletCryptoItem.Quantity,
                CurrentPrice = listingForWalletItem.Price,
                SubTotalValue = walletCryptoItem.Quantity * listingForWalletItem.Price,
            });
        }

        // Sum up
        portfolio.TotalValue = portfolio.PortfolioItems.Sum(it => it.SubTotalValue);

        return mapper.Map<PortfolioDto>(portfolio);
    }
}