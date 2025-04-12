using Microsoft.EntityFrameworkCore;

namespace CryptoSim.Model;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Crypto> Cryptos { get; set; }
    public DbSet<CryptoListing> CryptoListings { get; set; }
    public DbSet<CryptoTransaction> CryptoTransactions { get; set; }
}