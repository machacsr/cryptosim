using CryptoSim.Model;

namespace CryptoSim.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Wallet> WalletRepository { get; }
        IGenericRepository<CryptoTransaction> CryptoTransactionRepository { get; }
        IGenericRepository<CryptoListing> CryptoListingRepository { get; }
        IGenericRepository<Crypto> CryptoRepository { get; }

        void Save();
        Task SaveAsync();
    }
}
