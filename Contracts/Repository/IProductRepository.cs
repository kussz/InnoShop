using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts(bool trackChanges = false);
        Product GetProductById(int id);
    }
}
