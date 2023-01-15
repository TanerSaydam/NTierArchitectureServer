using Microsoft.EntityFrameworkCore.ChangeTracking;
using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.Entities.Models.Base;

namespace NTierArchitectureServer.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            UpdateAuditableEntities();
            int result = await _context.SaveChangesAsync();
            return result;
        }

        private void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry<BaseEntity>> entries =
                _context
                .ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entityEntry in entries)
            {
                if(entityEntry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    entityEntry.Property(p => p.CreatedDate)
                        .CurrentValue = DateTime.Now;
                }

                if(entityEntry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    entityEntry.Property(p => p.ModifiedDate)
                        .CurrentValue = DateTime.Now;
                }
            }
        }
    }
}
