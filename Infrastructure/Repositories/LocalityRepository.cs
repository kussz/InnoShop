using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<User> GetAllUsers(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public User GetUser(int id)
        {
            _cache.TryGetValue(id, out User user);
            if (user == null)
            {
                user = FindByCondition(u => u.Id == id).Single();
                _cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return user;
        }
    }
}
