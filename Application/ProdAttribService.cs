using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class ProdAttribService(IRepositoryManager repo) : IProdAttribService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<ProdAttrib> GetAllProdAttribs(bool trackChanges = false)
        {
            return this.Repository.ProdAttribRepository.GetAllProdAttribs(trackChanges);
        }
        public ProdAttrib GetProdAttrib(int id)
        {
            return Repository.ProdAttribRepository.GetProdAttribById(id);
        }
        public List<ProdAttrib> GetAttribsFromProduct(int id, bool trackChanges = false)
        {
            return Repository.ProdAttribRepository.GetProdAttribsFromProduct(id,trackChanges);
        }
    }   
}
