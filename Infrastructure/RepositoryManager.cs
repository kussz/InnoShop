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
        private readonly Lazy<IUserRepository>          _userRepository;
        private readonly Lazy<IUserTypeRepository>      _userTypeRepository;
        private readonly Lazy<IProductRepository>       _productRepository;
        private readonly Lazy<ILocalityRepository>      _localityRepository;
        private readonly Lazy<IProdTypeRepository>      _prodTypeRepository;
        private readonly Lazy<IProdAttribRepository>    _prodAttribRepository;

        public RepositoryManager(InnoShopContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context,cache));
            _userTypeRepository    = new Lazy<IUserTypeRepository>  (() => new UserTypeRepository(_context,cache));
            _productRepository     = new Lazy<IProductRepository>   (() => new ProductRepository(_context,cache));
            _localityRepository    = new Lazy<ILocalityRepository>  (() => new LocalityRepository(_context,cache));
            _prodTypeRepository    = new Lazy<IProdTypeRepository>  (() => new ProdTypeRepository(_context,cache));
            _prodAttribRepository  = new Lazy<IProdAttribRepository>(() => new ProdAttribRepository(_context,cache));
        }
        public IUserRepository UserRepository => _userRepository.Value;
        public IUserTypeRepository UserTypeRepository => _userTypeRepository.Value;
        public IProductRepository       ProductRepository     => _productRepository.Value;
        public ILocalityRepository      LocalityRepository    => _localityRepository.Value;
        public IProdTypeRepository      ProdTypeRepository    => _prodTypeRepository.Value;
        public IProdAttribRepository    ProdAttribRepository  => _prodAttribRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void SaveChanges() => _context.SaveChanges();
    }
}
