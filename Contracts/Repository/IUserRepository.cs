using InnoShop.Domain.Models;
namespace InnoShop.Contracts.Repository
{
    public interface IUserRepository
    {
        List<User> GetAllUsers(bool trackChanges = false);
        User GetUser(int id);
    }
}
