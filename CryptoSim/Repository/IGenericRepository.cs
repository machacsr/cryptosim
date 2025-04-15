using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoSim.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null);
        TEntity? GetById(object id, string[]? includeReferences = null, string[]? includeCollections = null);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null);
        Task<TEntity?> GetByIdAsync(object id, string[]? includeReferences = null, string[]? includeCollections = null);
        Task<TEntity> InsertAsync(TEntity entity);
    }
}
