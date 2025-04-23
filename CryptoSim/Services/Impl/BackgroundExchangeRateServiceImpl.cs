using CryptoSim.Model;
using CryptoSim.Repository;
using Microsoft.EntityFrameworkCore;

namespace CryptoSim.Services.Impl;

public class BackgroundExchangeRateServiceImpl(ILogger<BackgroundExchangeRateServiceImpl> logger, IUnitOfWork unitOfWork, AppDbContext dbContext) : BackgroundExchangeRateService
{
    private readonly string _instanceId = Guid.NewGuid().ToString();
    
    public async Task<Task> UpdateExchangeRateAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{ServiceName} doing work, instance ID: {Id}",
            nameof(BackgroundExchangeRateServiceImpl),
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
                Price = new Random().Next(1, 100_000), // TODO sofisticate based on listing group..
                State = CryptoListingState.Active
            });
        }
        
        // begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken: stoppingToken);

        // archive old
        await dbContext.CryptoListings
            .Where(listing => listing.State == CryptoListingState.Active)
            .ExecuteUpdateAsync(prop => prop.SetProperty(e => e.State, e => CryptoListingState.Archived),
                cancellationToken: stoppingToken);

        // insert new active listings
        dbContext.CryptoListings.AddRange(newListings);
        
        await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
        await transaction.CommitAsync(cancellationToken: stoppingToken);

        
        return Task.CompletedTask;
    }
}