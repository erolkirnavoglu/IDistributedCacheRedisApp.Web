using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;
using System.Linq;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<Product> CreateAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
