using AutoMapper;
using CryptoSim.Model;

namespace CryptoSim.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>();
        
        
        CreateMap<Wallet, WalletDto>();
        
        CreateMap<CryptoTransaction, CryptoTransactionDto>();

        CreateMap<Crypto, CryptoDto>();
        CreateMap<CreateCryptoDto, Crypto>();

        CreateMap<CryptoListing, CryptoListingDto>();
        CreateMap<CryptoListing, CryptoListingMarketplaceDto>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Crypto.Symbol))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Crypto.Name));
    }
}