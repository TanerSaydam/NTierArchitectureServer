using NTierArchitectureServer.Entities.Models.Base;

namespace NTierArchitectureServer.Entities.Models
{
    public sealed class EmailTemplate : BaseEntity
    {
        public string Title { get; set; }
        public byte[] Content { get; set; }
    }
}
