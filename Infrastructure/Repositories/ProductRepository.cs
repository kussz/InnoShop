using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<Product> GetAllProducts(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public Product GetProductById(int id)
        {
            _cache.TryGetValue(id, out Product product);
            if (product == null)
            {
                product = FindByCondition(u => u.Id == id).Single();
                _cache.Set(product.Id, product, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("Product извлечен из базы");
            }
            else
                Console.WriteLine("Product извлечен из кэша");
            return product;
        }
    }
}
