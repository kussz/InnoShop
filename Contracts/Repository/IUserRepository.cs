using InnoShop.Domain.Models;
using System.Linq.Expressions;
namespace InnoShop.Contracts.Repository
{
    public interface IUserRepository
    {
        List<User> GetAllUsers(bool trackChanges = false);
        User GetUser(int id);
        List<User> GetUsersByCondition(Expression<Func<User, bool>> expression, bool trackChanges);
    }
}
