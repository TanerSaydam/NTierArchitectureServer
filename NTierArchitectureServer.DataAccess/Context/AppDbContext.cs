using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NTierArchitectureServer.Entities.Models.Identity;

namespace NTierArchitectureServer.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {    
            builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);           
            builder.Ignore<IdentityUserLogin<string>>();            
            builder.Ignore<IdentityUserRole<string>>();            
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
        }
    }
}
