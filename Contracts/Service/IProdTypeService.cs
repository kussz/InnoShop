using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IProdTypeService
    {
        public List<ProdType> GetAllProdTypes(bool trackChanges = false);
        public ProdType GetProdType(int id);
        public void Add(ProdType locality);
        public void Edit(ProdType locality);
        public void Remove(ProdType locality);
    }
}
