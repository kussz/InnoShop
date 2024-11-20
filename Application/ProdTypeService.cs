using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class ProdTypeService(IRepositoryManager repo) : IProdTypeService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<ProdType> GetAllProdTypes(bool trackChanges = false)
        {
            return this.Repository.ProdTypeRepository.GetAllProdTypes(trackChanges);
        }
        public ProdType GetProdType(int id)
        {
            return Repository.ProdTypeRepository.GetProdTypeById(id);
        }
        public void Add(ProdType product) => Repository.ProdTypeRepository.Add(product);
        public void Edit(ProdType product) => Repository.ProdTypeRepository.Edit(product);
        public void Remove(ProdType locality) => Repository.ProdTypeRepository.Remove(locality);
    }   
}
