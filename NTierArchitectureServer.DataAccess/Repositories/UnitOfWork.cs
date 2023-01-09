using NTierArchitectureServer.DataAccess.Context;

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
            int result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
