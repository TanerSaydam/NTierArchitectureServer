using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.AuthServices.Dtos
{
    public class LoginDto
    {
        public string EmailorUserName { get; set; }
        public string Password { get; set; }
    }
}
