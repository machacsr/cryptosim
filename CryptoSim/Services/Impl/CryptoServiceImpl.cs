using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;

namespace CryptoSim.Services.Impl;

public class CryptoServiceImpl(IUnitOfWork unitOfWork, IMapper mapper) : CryptoService
{
    public async Task<CryptoDto> GetAllCryptoAsync()
    {
        var cryptos = await unitOfWork.CryptoRepository.GetAllAsync(null, ["CryptoListing"]);
        
        return mapper.Map<CryptoDto>(cryptos);
    }

    public async Task<CryptoDto> GetCryptoAsync(int cryptoId)
    {
        return mapper.Map<CryptoDto>(await unitOfWork.CryptoRepository.GetByIdAsync(cryptoId, null, ["CryptoListing"]));
    }

    public async Task<CryptoDto> CreateCryptoAsync(CryptoDto cryptoDto)
    {
        var crypto = mapper.Map<Crypto>(cryptoDto);
        crypto = await unitOfWork.CryptoRepository.InsertAsync(crypto);
        await unitOfWork.SaveAsync();
   
        return mapper.Map<CryptoDto>(crypto);
    }

    public async Task DeleteCryptoAsync(int cryptoId)
    {
        var crypto = await unitOfWork.CryptoRepository.GetByIdAsync(cryptoId);
        if (crypto != null) unitOfWork.CryptoRepository.Delete(crypto);
        await unitOfWork.SaveAsync();
    }
}