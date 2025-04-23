using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;

namespace CryptoSim.Services.Impl;

public class CryptoServiceImpl(IUnitOfWork unitOfWork, IMapper mapper) : CryptoService
{
    public async Task<List<CryptoListingMarketplaceDto>> GetAllLatestCryptoListingAsync()
    {
        var cryptos = await unitOfWork.CryptoRepository.GetAllAsync(null, ["CryptoListings"]);

        var latestListingItemForEachCrypto = cryptos.Select(item => item.CryptoListings.Find(listing => listing.State == CryptoListingState.Active))
            .ToList();

        return latestListingItemForEachCrypto.Select(mapper.Map<CryptoListingMarketplaceDto>).ToList();
    }

    public async Task<CryptoDto> GetCryptoAsync(int cryptoId)
    {
        return mapper.Map<CryptoDto>(await unitOfWork.CryptoRepository.GetByIdAsync(cryptoId, null, ["CryptoListings"]));
    }

    public async Task<CryptoDto> CreateCryptoAsync(CreateCryptoDto cryptoDto)
    {
        // Initial price must be greater than zero
        if (cryptoDto.InitialPrice <= 0)
        {
            throw new BadRequestException("Validation Exception", "Price must be greater than zero");
        }
        
        // Symbol must be unique
        var cryptos = await unitOfWork.CryptoRepository.GetAllAsync(crypto => crypto.Symbol == cryptoDto.Symbol);
        if (cryptos.ToList().Count > 0)
        {
            throw new BadRequestException("Validation Exception", "Symbol must be unique");
        }
        
        //create entity
        var crypto = mapper.Map<Crypto>(cryptoDto);
        crypto = await unitOfWork.CryptoRepository.InsertAsync(crypto);
        var initialListing = new CryptoListing()
        {
            Crypto = crypto,
            State = CryptoListingState.Active,
            Price = cryptoDto.InitialPrice,
            CryptoId = crypto.Id,
        };
        await unitOfWork.CryptoListingRepository.InsertAsync(initialListing);
        await unitOfWork.SaveAsync();
        return mapper.Map<CryptoDto>(crypto);
    }

    
    // maybe better with logical delete ???
    public async Task DeleteCryptoAsync(int cryptoId)
    {
        var crypto = await unitOfWork.CryptoRepository.GetByIdAsync(cryptoId);
        if (crypto != null) unitOfWork.CryptoRepository.Delete(crypto); // cascade
        await unitOfWork.SaveAsync();
    }
}