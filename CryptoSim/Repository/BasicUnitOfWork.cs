using CryptoSim.Model;

namespace CryptoSim.Repository
{
    public class BasicUnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly GenericRepository<User> _userRepository = new(context);
        private readonly GenericRepository<Wallet> _walletRepository = new(context);
        private readonly GenericRepository<CryptoTransaction> _cryptoTransactionRepository = new(context);
        private readonly GenericRepository<CryptoListing> _cryptoListingRepository = new(context);
        private readonly GenericRepository<Crypto> _cryptoRepository = new(context);

        public IGenericRepository<User> UserRepository => _userRepository;
        public IGenericRepository<Wallet> WalletRepository => _walletRepository;
        public IGenericRepository<CryptoTransaction> CryptoTransactionRepository => _cryptoTransactionRepository;
        public IGenericRepository<CryptoListing> CryptoListingRepository => _cryptoListingRepository;
        public IGenericRepository<Crypto> CryptoRepository => _cryptoRepository;

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
