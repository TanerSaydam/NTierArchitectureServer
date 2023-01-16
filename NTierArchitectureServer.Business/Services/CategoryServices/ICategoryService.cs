using NTierArchitectureServer.Business.Services.CategoryServices.Dtos;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task AddAsync(AddCategoryDto addCategoryDto);
        IQueryable<Category> GetAll();
    }
}
