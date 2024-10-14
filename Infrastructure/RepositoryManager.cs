using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using InnoShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Infrastructure
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IMemoryCache _cache;
        private readonly InnoShopContext _context;
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(InnoShopContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context,cache));
        }
        public IUserRepository UserRepository => _userRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void SaveChanges() => _context.SaveChanges();
    }
}
