using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IProductService
    {
        public List<Product> GetAllProducts(bool trackChanges = false);
        public Product GetProduct(int id);
        public List<Product> GetCachedProducts();
        public List<Product> GetProductsByCondition(Expression<Func<Product,bool>> expression, bool trackChanges = false);
    }
}
