using APITEST.Common.Interfaces;
using APITEST.Data;
using APITEST.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APITEST.Modules.ProductsCategory.Repository
{
    public class ProductCategoryRepository : IRepository<ProductCategory>
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<ProductCategory>> GetAll() =>
            await _context.ProductCategories.ToListAsync();

        public async Task<ProductCategory> GetById(int Id) =>
            await _context.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Id == Id);

        public async Task Create(ProductCategory productCategory) =>
            await _context.ProductCategories.AddAsync(productCategory);

        public void Update(ProductCategory productCategory)
        {
            _context.ProductCategories.Attach(productCategory);
            _context.ProductCategories.Entry(productCategory).State = EntityState.Modified;
        }
        public void Delete(ProductCategory productCategory) 
            => _context.ProductCategories.Remove(productCategory);

        public async Task Save() => await _context.SaveChangesAsync();

        public IEnumerable<ProductCategory> Search(Func<ProductCategory, bool> filter) =>
            _context.ProductCategories.Where(filter).ToList();

        public ProductCategory SearchOne(Func<ProductCategory, bool> filter) =>
            _context.ProductCategories.Where(filter).FirstOrDefault();
    }
}
