using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface ILocalityService
    {
        public List<Locality> GetAllLocalities(bool trackChanges = false);
        public Locality GetLocality(int id);
    }
}
