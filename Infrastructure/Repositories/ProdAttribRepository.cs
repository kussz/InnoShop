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
            var prodAttrib = FindByCondition(u => u.Id == id).Single();
            return prodAttrib;
        }
        public List<ProdAttrib> GetProdAttribsFromProduct(int id, bool trackChanges = false) => FindByCondition(attrib => attrib.ProdId == id,trackChanges).ToList();
        public void Add(ProdAttrib product)
        {
            Create(product);
            Save();
        }
        public void Remove(ProdAttrib product)
        {
            Delete(product);
            Save();
        }
        public void Clear(int prodId)
        {
            var toDelete = FindByCondition(p => p.ProdId == prodId);
            Delete(toDelete);
            Save();
        }
    }
}
