using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Core.Utilies
{
    public static class FileControlService
    {
        public static void CheckFileSize(IFormFile file, int checkSize)
        {
            double mbFileSize = (file.Length * 0.000001);
            if (mbFileSize > checkSize) throw new Exception($"Dosya boyutu maximum {checkSize} mb olabilir!");
        }

        public static void CheckFileType(IFormFile file, List<string> types)
        {                        
            if (!types.Contains(file.ContentType)) 
                throw new Exception($"Eklediğiniz dosya izin verilen formatlarda değil! İzin verilen formatlar {String.Join(", ", types)}");
        }
    }    
}
