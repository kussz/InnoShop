﻿using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InnoShop.Infrastructure.Repositories
{
    public class LocalityRepository : RepositoryBase<Locality>, ILocalityRepository
    {
        public LocalityRepository(InnoShopContext context,IMemoryCache cache) : base(context, cache)
        { }
        public List<Locality> GetAllLocalities(bool trackChanges = false) => FindAll(trackChanges).ToList();
        public Locality GetLocalityById(int id)
        {
            _cache.TryGetValue(id, out Locality locality);
            if (locality == null)
            {
                locality = FindByCondition(u => u.Id == id).Single();
                _cache.Set(locality.Id, locality, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(294)));
                Console.WriteLine("User извлечен из базы");
            }
            else
                Console.WriteLine("User извлечен из кэша");
            return locality;
        }
    }
}
