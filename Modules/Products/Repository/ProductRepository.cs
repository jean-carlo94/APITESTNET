using Microsoft.EntityFrameworkCore;
using APITEST.Data;
using APITEST.Common.Interfaces;
using APITEST.Models;

namespace APITEST.Modules.Products.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll() =>
            await _context.Products.ToListAsync();

        public async Task<Product> GetById(int Id) =>
            await _context.Products.FindAsync(Id);

        public async Task Create(Product product) =>
            await _context.Products.AddAsync(product);

        public void Update(Product product)
        {
            _context.Products.Attach(product);
            _context.Products.Entry(product).State = EntityState.Modified;
        }

        public void Delete(Product product) =>
            _context.Products.Remove(product);

        public async Task Save() => await _context.SaveChangesAsync();

        public IEnumerable<Product> Search(Func<Product, bool> filter) =>
            _context.Products.Where(filter).ToList();

        public Product SearchOne(Func<Product, bool> filter) =>
            _context.Products.Where(filter).FirstOrDefault();
    }
}
