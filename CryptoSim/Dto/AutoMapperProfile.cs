using AutoMapper;
using CryptoSim.Model;

namespace CryptoSim.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>();

    }
}