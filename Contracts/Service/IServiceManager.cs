using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IUserTypeService UserTypeService { get; }
        IProductService ProductService { get; }
        ILocalityService LocalityService { get; }
        IProdTypeService ProdTypeService { get; }
        IProdAttribService ProdAttribService { get; }
    }
}
