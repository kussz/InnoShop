using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class LocalityService(IRepositoryManager repo) : ILocalityService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<Locality> GetAllLocalities(bool trackChanges = false)
        {
            return this.Repository.LocalityRepository.GetAllLocalities(trackChanges);
        }
        public Locality GetLocality(int id)
        {
            return Repository.LocalityRepository.GetLocalityById(id);
        }
        public void Add(Locality product) => Repository.LocalityRepository.Add(product);
        public void Edit(Locality product) => Repository.LocalityRepository.Edit(product);
        public void Remove(Locality locality)=>Repository.LocalityRepository.Remove(locality);
    }   
}
