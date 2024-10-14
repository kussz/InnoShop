using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class UserService(IRepositoryManager repo) : IUserService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<User> GetAllUsers(bool trackChanges = false)
        {
            return this.Repository.UserRepository.GetAllUsers(trackChanges);
        }
        public User GetUser(int id)
        {
            return Repository.UserRepository.GetUser(id);
        }
    }   
}
