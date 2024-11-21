using InnoShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IUserService
    {
        public List<User> GetAllUsers(bool trackChanges = false);
        public User GetUser(int id);
        public List<User> GetUsersByCondition(Expression<Func<User, bool>> expression, bool trackChanges = false);
        User? Authorize(string? fullToken);
        string? GetRole(string? fullToken);
    }
}
