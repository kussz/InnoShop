using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IUserService
    {
        public List<User> GetAllUsers(bool trackChanges = false);
        public User GetUser(int id);
    }
}
