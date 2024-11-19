using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace InnoShop.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<User> GetAllUsers(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public User GetUser(int id)
        {
            //_cache.TryGetValue(id, out User user);
            //if (user == null)
            //{
                var user = FindByCondition(u => u.Id == id,true).Include(p=>p.Locality).Include(p=>p.UserType).Single();
                _cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            //}
            //else
            //    Console.WriteLine("User извлечен из кэша");
            return user;
        }
        public List<User> GetUsersByCondition(Expression<Func<User, bool>> expression, bool trackChanges)
        {
            var users = FindByCondition(expression, trackChanges).ToList();
            return users;
        }
    }
}
