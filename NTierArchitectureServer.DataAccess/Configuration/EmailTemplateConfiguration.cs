using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTierArchitectureServer.DataAccess.Constants;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.DataAccess.Configuration
{
    public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.ToTable(TableNames.EmailTemplates);
            builder.HasKey(p => p.Id);
        }
    }
}
