using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using System.Linq.Expressions;

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
        public List<User> GetUsersByCondition(Expression<Func<User, bool>> expression, bool trackChanges)
        {
            return Repository.UserRepository.GetUsersByCondition(expression, trackChanges);
        }
    }   
}
