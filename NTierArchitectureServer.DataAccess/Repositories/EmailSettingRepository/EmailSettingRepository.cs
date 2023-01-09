using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.DataAccess.Repositories.EmailSettingRepository
{
    public class EmailSettingRepository : Repository<EmailSetting>, IEmailSettingRepository
    {
        public EmailSettingRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
