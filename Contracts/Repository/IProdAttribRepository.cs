using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IProdAttribRepository
    {
        List<ProdAttrib> GetAllProdAttribs(bool trackChanges = false);
        List<ProdAttrib> GetProdAttribsFromProduct(int id,bool trackChanges = false);
        ProdAttrib GetProdAttribById(int id);
        void Add(ProdAttrib product);
        void Remove(ProdAttrib product);
        void Clear(int prodId);
    }
}
