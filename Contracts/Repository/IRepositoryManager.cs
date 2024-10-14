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
        void SaveChanges();
        Task SaveAsync();
    }
}
