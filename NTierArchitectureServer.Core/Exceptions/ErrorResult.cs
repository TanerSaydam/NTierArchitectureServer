using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Core.Exceptions
{
    public sealed class ErrorResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
