using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NTierArchitectureServer.Business.Services.CategoryServices.Dtos;
using NTierArchitectureServer.Business.Services.Logs.LogCategoryServices;
using NTierArchitectureServer.DataAccess.Repositories;
using NTierArchitectureServer.DataAccess.Repositories.CategoryRepository;
using NTierArchitectureServer.Entities.Models;
using System.Runtime.CompilerServices;

namespace NTierArchitectureServer.Business.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogCategoryService _logCategoryService;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ILogCategoryService logCategoryService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logCategoryService = logCategoryService;
        }

        public async Task AddAsync(AddCategoryDto addCategoryDto)
        {
            var checkCategoryName = await _categoryRepository.GetWhere(p=> p.Name == addCategoryDto.Name).FirstOrDefaultAsync();
            if (checkCategoryName != null) throw new Exception("Bu kategori adı daha önce kullanılmış!");

            Category category = new()
            {
                Id = Guid.NewGuid(),
                Name = addCategoryDto.Name,
            };
            await _categoryRepository.AddAsync(category);

            await _logCategoryService.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }
    }
}
