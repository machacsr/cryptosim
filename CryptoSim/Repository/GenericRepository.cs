
using System.Linq.Expressions;
using CryptoSim.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoSim.Repository
{
    public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbset;
        
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }
        
        
        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (includeProperties != null)
            {
                foreach (string property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (includeProperties != null)
            {
                foreach (string property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public TEntity? GetById(object id, string[]? includeReferences = null, string[]? includeCollections = null)
        {
            TEntity? entity = _dbset.Find(id);
            if (entity == null)
            {
                return null;
            }
            if (includeReferences != null)
            {
                foreach (var reference in includeReferences)
                {
                    _context.Entry(entity).Reference(reference).Load();
                }
            }
            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    _context.Entry(entity).Collection(collection).Load();
                }
            }
            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(object id, string[]? includeReferences = null, string[]? includeCollections = null)
        {
            TEntity? entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            List<Task> loadTasks = new List<Task>();
            if (includeReferences != null)
            {
                foreach (var reference in includeReferences)
                {
                    loadTasks.Add(_context.Entry(entity).Reference(reference).LoadAsync());
                }
            }
            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    loadTasks.Add(_context.Entry(entity).Collection(collection).LoadAsync());
                }
            }
            await Task.WhenAll(loadTasks);
            return entity;
        }

        public TEntity Insert(TEntity entity)
        {
            return _dbset.Add(entity).Entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            return (await _dbset.AddAsync(entity)).Entity;
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
            //_context.Entry(entity).State = EntityState.Modified;
        }
    }
}
