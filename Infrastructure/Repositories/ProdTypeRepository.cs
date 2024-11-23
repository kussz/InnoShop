using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class ProdTypeRepository : RepositoryBase<ProdType>, IProdTypeRepository
    {
        public ProdTypeRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<ProdType> GetAllProdTypes(bool trackChanges = false) => FindAll(trackChanges).OrderBy(p => p.Id).ToList();
        public ProdType GetProdTypeById(int id)
        {
            _cache.TryGetValue("prodType"+id, out ProdType prodType);
            if (prodType == null)
            {
                prodType = FindByCondition(u => u.Id == id).Single();
                _cache.Set("prodType"+prodType.Id, prodType, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return prodType;
        }
        public void Edit(ProdType locality)
        {
            Update(locality);
            _cache.Set("prodType" + locality.Id, locality, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
            Save();
        }
        public void Add(ProdType product)
        {
            Create(product);
            Save();
        }
        public void Remove(ProdType locality)
        {
            Delete(locality);
            _cache.Remove("prodType" + locality.Id);
            Save();
        }
    }
}
