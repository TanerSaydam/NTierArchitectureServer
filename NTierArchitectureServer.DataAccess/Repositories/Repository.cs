using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DbSet<T> Entity => _appDbContext.Set<T>();

        #region Commands
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Entity.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await Entity.AddRangeAsync(entities);
        }        

        public bool Remove(T entity)
        {
            EntityEntry<T> entityEntry = Entity.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveByExpressionAsync(Expression<Func<T, bool>> expression)
        {
            T entity = await Entity.FirstOrDefaultAsync(expression);
            return Remove(entity);
        }

        public async Task<bool> RemoveByIdAsync(string id)
        {
            T entity = await Entity.FindAsync(Guid.Parse(id));
            return Remove(entity);
        }

        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Entity.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
        #endregion

        #region Queries
        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            if (!tracking) { 
                var query = await Entity.AsNoTracking().FirstOrDefaultAsync(expression);
                return query;
            }
            return await Entity.FirstOrDefaultAsync(expression);            
        }

        public async Task<T> GetFirstAsync(bool tracking = true)
        {
            if (!tracking)
            {
                var query = await Entity.AsNoTracking().FirstOrDefaultAsync();
                return query;
            }
            return await Entity.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Entity.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var query = Entity.Where(expression);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            if (!tracking)
                return await Entity.AsNoTracking().FirstOrDefaultAsync(p=> p.Id == Guid.Parse(id));
            return await Entity.FindAsync(Guid.Parse(id));
        }
        #endregion



    }
}
