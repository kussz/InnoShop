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
        public List<ProdType> GetAllProdTypes(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public ProdType GetProdTypeById(int id)
        {
            _cache.TryGetValue(id, out ProdType prodType);
            if (prodType == null)
            {
                prodType = FindByCondition(u => u.Id == id).Single();
                _cache.Set(prodType.Id, prodType, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return prodType;
        }
    }
}
