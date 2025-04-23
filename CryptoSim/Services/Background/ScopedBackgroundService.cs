namespace CryptoSim.Services.Background;

public sealed class ScopedBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ScopedBackgroundService> logger) : BackgroundService
{
    private const string ClassName = nameof(ScopedBackgroundService);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{Name} is running.", ClassName);

        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();

            BackgroundExchangeRateService scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<BackgroundExchangeRateService>();

            await scopedProcessingService.UpdateExchangeRateAsync(stoppingToken);

            await Task.Delay(10_000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{Name} is stopping.", ClassName);

        await base.StopAsync(stoppingToken);
    }
}