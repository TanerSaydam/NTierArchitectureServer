using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTierArchitectureServer.DataAccess.Constants;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.DataAccess.Configuration
{
    internal sealed class EmailSettingConfiguration : IEntityTypeConfiguration<EmailSetting>
    {
        public void Configure(EntityTypeBuilder<EmailSetting> builder)
        {
            builder.ToTable(TableNames.EmailSettings); //Table adı
            builder.HasKey(e => e.Id); //primary key
        }
    }
}
