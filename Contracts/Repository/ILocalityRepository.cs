using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface ILocalityRepository
    {
        void Add(Locality locality);
        void Edit(Locality locality);
        void Remove(Locality locality);
        List<Locality> GetAllLocalities(bool trackChanges = false);
        Locality GetLocalityById(int id);
    }
}
