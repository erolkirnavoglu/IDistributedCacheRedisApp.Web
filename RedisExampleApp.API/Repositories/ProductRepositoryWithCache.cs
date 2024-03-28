

using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private const string productKey = "productCache";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDatabase(5);
        }
        public async Task<Product> CreateAsync(Product entity)
        {
            var newProduct = await _productRepository.CreateAsync(entity);

            await _cacheRepository.HashSetAsync(productKey, newProduct.Id, JsonSerializer.Serialize(newProduct));

            return newProduct;

        }

        public async Task<List<Product>> GetAllAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetAsync(int id)
        {
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var cacheProduct = await _cacheRepository.HashGetAsync(productKey, id);
                return cacheProduct.HasValue ? JsonSerializer.Deserialize<Product>(cacheProduct) : null;
            }
            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(p => p.Id == id);

        }
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var product = await _productRepository.GetAllAsync();
            product.ForEach(p =>
            {

                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return product;

        }
    }
}
