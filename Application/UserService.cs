using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;

namespace InnoShop.Application
{
    public class UserService(IUserRepository repo) : IUserService
    {
        private readonly IUserRepository userRepository = repo;
        public List<User> GetAllUsers(bool trackChanges = false)
        {
            return this.userRepository.GetAllUsers(trackChanges);
        }
        public User GetUser(int id)
        {
            return userRepository.GetUser(id);
        }
    }   
}
