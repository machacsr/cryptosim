using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CryptoSim.Services.Impl;

public class ExchangeRateServiceImpl(
    ILogger<ExchangeRateServiceImpl> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    AppDbContext dbContext) : ExchangeRateService
{
    private readonly string _instanceId = Guid.NewGuid().ToString();

    /// <summary>
    /// Update all cryptoListing with new price.
    /// </summary>
    public async Task<Task> UpdateExchangeRateAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{ServiceName} doing work, instance ID: {Id}",
            nameof(ExchangeRateServiceImpl),
            _instanceId);

        var listings = await unitOfWork.CryptoListingRepository.GetAllAsync(null, ["Crypto"]);

        var groupByCrypto = listings.GroupBy(c => c.Crypto)
            .ToDictionary(g => g.Key, g => g.ToList());

        var newListings = new List<CryptoListing>();
        foreach (var listingGroup in groupByCrypto)
        {
            newListings.Add(new CryptoListing()
            {
                Crypto = listingGroup.Key,
                CryptoId = listingGroup.Key.Id,
                Price = new Random().Next(1, 1_000), // TODO sofisticate based on listing group..
                State = CryptoListingState.Active
            });
        }

        // begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken: stoppingToken);

        // archive old
        await dbContext.CryptoListings
            .Where(listing => listing.State == CryptoListingState.Active)
            .ExecuteUpdateAsync(prop => prop
                .SetProperty(e => e.State, CryptoListingState.Archived)
                .SetProperty(e => e.ArchivedAt, DateTime.Now),
                cancellationToken: stoppingToken);

        // insert new active listings
        dbContext.CryptoListings.AddRange(newListings);

        await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
        await transaction.CommitAsync(cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Create new listing item for the given crypto.
    /// </summary>
    public async Task<CryptoListingDto> CreateCryptoListingAsync(CreateCryptoListingDto cryptoListingDto)
    {
        if (cryptoListingDto.NewPrice <= 0)
        {
            throw new BadRequestException("Validation error", "Price must be greater than zero");
        }

        var crypto =
            await unitOfWork.CryptoRepository.GetByIdAsync(cryptoListingDto.CryptoId, null, ["CryptoListings"]);
        if (crypto == null)
        {
            throw new BadRequestException("Validation error", "Crypto not found");
        }

        // begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        // archive old
        await dbContext.CryptoListings
            .Where(listing =>
                listing.State == CryptoListingState.Active && listing.CryptoId == cryptoListingDto.CryptoId)
            .ExecuteUpdateAsync(prop => prop
                    .SetProperty(e => e.State, CryptoListingState.Archived)
                    .SetProperty(e => e.ArchivedAt, DateTime.Now));

        // insert new active listings
        var newListing = new CryptoListing()
        {
            CryptoId = cryptoListingDto.CryptoId,
            Price = cryptoListingDto.NewPrice,
            State = CryptoListingState.Active,
        };

        newListing = await unitOfWork.CryptoListingRepository.InsertAsync(newListing);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return mapper.Map<CryptoListingDto>(newListing);
    }

    public async Task<List<CryptoListingDto>> GetListingsForCrypto(int cryptoId)
    {
        var crypto = await unitOfWork.CryptoRepository.GetByIdAsync(cryptoId, null, ["CryptoListings"]);

        if (crypto == null)
        {
            throw new BadRequestException("Validation error", "Crypto not found");
        }

        return crypto.CryptoListings.Select(mapper.Map<CryptoListingDto>).ToList();
    }
}