using NTierArchitectureServer.Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Core.Security
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}
