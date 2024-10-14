using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class ProdAttribRepository : RepositoryBase<ProdAttrib>, IProdAttribRepository
    {
        public ProdAttribRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<ProdAttrib> GetAllProdAttribs(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public ProdAttrib GetProdAttribById(int id)
        {
            _cache.TryGetValue(id, out ProdAttrib prodAttrib);
            if (prodAttrib == null)
            {
                prodAttrib = FindByCondition(u => u.Id == id).Single();
                _cache.Set(prodAttrib.Id, prodAttrib, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return prodAttrib;
        }
        public List<ProdAttrib> GetProdAttribsFromProduct(int id, bool trackChanges = false) => FindByCondition(attrib => attrib.ProdId == id,trackChanges).ToList();
    }
}
