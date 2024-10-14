using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IProdTypeRepository
    {
        List<ProdType> GetAllProdTypes(bool trackChanges = false);
        ProdType GetProdTypeById(int id);
    }
}
