using NTierArchitectureServer.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Entities.Models
{
    public sealed class Category: BaseEntity
    {
        public string Name { get; set; }
    }
}
