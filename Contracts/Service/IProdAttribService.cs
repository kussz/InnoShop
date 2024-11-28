using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IProdAttribService
    {
        public List<ProdAttrib> GetAllProdAttribs(bool trackChanges = false);
        public ProdAttrib GetProdAttrib(int id);
        public List<ProdAttrib> GetAttribsFromProduct(int id, bool trackChanges = false);
        public void Add(ProdAttrib product);
        public void Remove(ProdAttrib product);
        public void Clear(int prodId);
    }
}
