using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IUserTypeRepository UserTypeRepository { get; }
        IProductRepository ProductRepository { get; }
        ILocalityRepository LocalityRepository { get; }
        IProdTypeRepository ProdTypeRepository { get; }
        IProdAttribRepository ProdAttribRepository { get; }
        void SaveChanges();
        Task SaveAsync();
    }
}
