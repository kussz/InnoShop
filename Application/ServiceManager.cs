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
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _userService = new Lazy<IUserService>(() => new
            UserService(repositoryManager));

        }
        public IUserService UserService => _userService.Value;
    }
}
