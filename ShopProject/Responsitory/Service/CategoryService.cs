using AutoMapper;
using BussinessObject.Models;
using DataAccess.DB;
using Microsoft.EntityFrameworkCore;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly Db _dbContext;
        private readonly IMapper _mapper;
        public Category cate { get; set; }
        public List<Category> categories { get; set; }
        public CategoryService(Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateCategoryAsync(CategoryResponse categoryResponse)
        {
            cate = new Category()
            {
                CategoryId = categoryResponse.CategoryId,
                CategoryName = categoryResponse.CategoryName,
                CategoryStatus = "1"
            };

            _dbContext.Categorys.Add(cate);
            await _dbContext.SaveChangesAsync();

            return cate.CategoryId;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category detemp = _dbContext.Categorys.SingleOrDefault(c => c.CategoryId == id);
            Product tempro = _dbContext.Products.Where(c => c.CategoryID == detemp.CategoryId).SingleOrDefault();
            if(tempro != null)
            {
                throw new Exception();
            }
            detemp.CategoryStatus = "0";
            _dbContext.Categorys.Update(detemp);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CategoryResponse> GetByIdAsync(int id)
        {
            var category = await _dbContext.Categorys.FindAsync(id);
            if (category == null)
            {
                throw new Exception("allcate");
            }
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<List<CategoryResponse>> GetCategoryAsync()
        {
            var categorys = _dbContext.Categorys.Where(c => c.CategoryStatus == "1").ToList();
            if (categorys == null)
            {
                throw new Exception("allcate");
            }
            return _mapper.Map<List<CategoryResponse>>(categorys);
        }


        public async Task UpdateCategoryAsync(int id, CategoryResponse categoryResponse)
        {
            var check = _dbContext.Categorys.Where(a => a.CategoryId == id).FirstOrDefault();
            if (check == null)
            {
                throw new Exception("XXXXX");

            }
            if (id == categoryResponse.CategoryId)
            {
                Category category = _dbContext.Categorys.Where(a => a.CategoryId == id).FirstOrDefault();
                category.CategoryName = categoryResponse.CategoryName;
                _dbContext.Categorys.Update(category);
                await _dbContext.SaveChangesAsync();
            }}

        public List<Category> categorys { get; set; }
        public async Task<List<CategoryResponse>> SearchCategoryAsync(string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                categorys = _dbContext.Categorys.Where(p =>
                    p.CategoryName.Contains(search) && p.CategoryStatus == "1"
                ).ToList();
                if (categorys == null)
                {
                    throw new Exception("NULLRESULT");
                }
            }
            else
            {
                throw new Exception("searchnull");
            }

            return _mapper.Map<List<CategoryResponse>>(categorys);
        }
    }
}
