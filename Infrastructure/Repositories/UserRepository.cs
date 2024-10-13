using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;

namespace InnoShop.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(InnoShopContext context) : base(context)
        { }
        public List<User> GetAllUsers(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public User GetUser(int id) => FindByCondition(u => u.Id == id).Single();
    }
}
