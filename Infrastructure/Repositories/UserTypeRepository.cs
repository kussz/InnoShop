using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class UserTypeRepository : RepositoryBase<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<UserType> GetAllUserTypes(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public UserType GetUserTypeById(int id)
        {
            _cache.TryGetValue(id, out UserType userType);
            if (userType == null)
            {
                userType = FindByCondition(u => u.Id == id).Single();
                _cache.Set(userType.Id, userType, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return userType;
        }
    }
}
