using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class UserTypeService(IRepositoryManager repo) : IUserTypeService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<UserType> GetAllUserTypes(bool trackChanges = false)
        {
            return this.Repository.UserTypeRepository.GetAllUserTypes(trackChanges);
        }
        public UserType GetUserType(int id)
        {
            return Repository.UserTypeRepository.GetUserTypeById(id);
        }
    }   
}
