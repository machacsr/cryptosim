using Microsoft.EntityFrameworkCore;

namespace CryptoSim.Model;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Crypto> Cryptos { get; set; }
    public DbSet<CryptoListing> CryptoListings { get; set; }
    public DbSet<CryptoTransaction> CryptoTransactions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<CryptoListing>()
            .Property(e => e.State)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<CryptoListingState>(v));

        modelBuilder
            .Entity<CryptoTransaction>()
            .Property(e => e.TransactionType)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<CryptoTransactionType>(v));
    }
}