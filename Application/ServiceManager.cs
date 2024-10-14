using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IUserTypeService> _userTypeService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<ILocalityService> _localityService;
        private readonly Lazy<IProdTypeService> _prodTypeService;
        private readonly Lazy<IProdAttribService> _prodAttribService;
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager));
            _userTypeService   = new Lazy<IUserTypeService>(() => new UserTypeService(repositoryManager));
            _productService    = new Lazy<IProductService>(() => new ProductService(repositoryManager));
            _localityService   = new Lazy<ILocalityService>(() => new LocalityService(repositoryManager));
            _prodTypeService   = new Lazy<IProdTypeService>(() => new ProdTypeService(repositoryManager));
            _prodAttribService = new Lazy<IProdAttribService>(() => new ProdAttribService(repositoryManager));

        }
        public IUserService UserService => _userService.Value;
        public IUserTypeService UserTypeService => _userTypeService.Value;
        public IProductService ProductService => _productService.Value;
        public ILocalityService LocalityService => _localityService.Value;
        public IProdTypeService ProdTypeService => _prodTypeService.Value;
        public IProdAttribService ProdAttribService => _prodAttribService.Value;
    }
}
