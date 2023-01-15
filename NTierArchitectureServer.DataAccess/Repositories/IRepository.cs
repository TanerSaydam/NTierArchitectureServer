using Microsoft.EntityFrameworkCore;
using NTierArchitectureServer.Entities.Models.Base;
using System.Linq.Expressions;

namespace NTierArchitectureServer.DataAccess.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        #region Commands
        DbSet<T> Entity { get; }
        Task<bool> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        bool Remove(T entity);
        Task<bool> RemoveByIdAsync(string id);
        Task<bool> RemoveByExpressionAsync(Expression<Func<T, bool>> expression);
        #endregion

        #region Queries
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<T> GetFirstAsync(bool tracking = true);
        #endregion
    }
}
