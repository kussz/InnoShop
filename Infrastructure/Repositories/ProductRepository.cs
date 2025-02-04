﻿using Azure;
using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace InnoShop.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public void Add(Product product)
        {
            Create(product);
            _cache.Remove("Product"+product.Id);
            Save();
        }
        public void Edit(Product product)
        {
            Update(product);
            _cache.Remove("Product" + product.Id);
            Save();
        }
        public ProductRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<Product> GetAllProducts(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public Product GetProductById(int id)
        {
            _cache.TryGetValue(id, out Product product);
            if (product == null)
            {
                product = FindByCondition(u => u.Id == id).Include(p=>p.ProdType).Include(p=>p.User).Include(p=>p.Buyer).Single();
                _cache.Set("Product"+product.Id, product, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("Product извлечен из базы");
            }
            else
                Console.WriteLine("Product извлечен из кэша");
            return product;
        }
        public List<Product> GetProductByCondition(Expression<Func<Product,bool>> expression, bool trackChanges)
        {
            var products = FindByCondition(expression,trackChanges).ToList();
            _cache.Set("prods", products, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
            return products;
        }
        public List<Product> GetCachedProducts()
        {
            var products = Context.Products;
            if (_cache.TryGetValue("prods", out List<Product> cachedProds))
                 return cachedProds;
            else
                return new List<Product>();
        }
        public List<Product> GetPage(int quantity, int page)
        {
            var products = Paginate(quantity, page).Include(p => p.ProdType).ToList();
            Console.WriteLine("From db");
            return products;
        }
    }
}
