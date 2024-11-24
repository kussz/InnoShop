using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Edit(Product product);
        void Remove(Product locality);
        List<Product> GetAllProducts(bool trackChanges = false);
        Product GetProductById(int id);
        List<Product> GetCachedProducts();
        List<Product> GetProductByCondition(Expression<Func<Product, bool>> expression, bool trackChanges);
        List<Product> GetPage(int quantity, int page,ProductFilterDTO dto);
    }
}
