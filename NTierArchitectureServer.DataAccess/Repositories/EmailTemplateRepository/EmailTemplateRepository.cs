using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.DataAccess.Repositories.EmailTemplateRepository
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
