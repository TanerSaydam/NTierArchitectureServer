using NTierArchitectureServer.Entities.Models.Base;

namespace NTierArchitectureServer.Entities.Models
{
    public sealed class EmailSetting : BaseEntity
    {        
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public bool HTML { get; set; }
    }
}
