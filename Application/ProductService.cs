using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class ProductService(IRepositoryManager repo) : IProductService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<Product> GetAllProducts(bool trackChanges = false)
        {
            return this.Repository.ProductRepository.GetAllProducts(trackChanges);
        }
        public Product GetProduct(int id)
        {
            return Repository.ProductRepository.GetProductById(id);
        }
    }   
}
