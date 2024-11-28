using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using System.Linq.Expressions;

namespace InnoShop.Application
{
    public class ProductService(IRepositoryManager repo) : IProductService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<Product> GetAllProducts(bool trackChanges = false)
        {
            return this.Repository.ProductRepository.GetAllProducts(trackChanges);
        }
        public List<Product> GetProductsByCondition(Expression<Func<Product, bool>> expression, bool trackChanges)
        {
            return Repository.ProductRepository.GetProductByCondition(expression, trackChanges);
        }
        public Product GetProduct(int id)
        {
            return Repository.ProductRepository.GetProductById(id);
        }
        public List<Product> GetCachedProducts()
        {
            return Repository.ProductRepository.GetCachedProducts();
        }
        public List<Product> GetPage(int quantity, int page,ProductFilterDTO dto)
        {
            return Repository.ProductRepository.GetPage(quantity, page,dto);
        }
        public void Add(Product product) => Repository.ProductRepository.Add(product);
        public void Remove(Product product) => Repository.ProductRepository.Remove(product);
        public void Edit(Product product) => Repository.ProductRepository.Edit(product);
    }   
}
