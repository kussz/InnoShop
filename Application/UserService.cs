using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;

namespace InnoShop.Application
{
    public class UserService(IRepositoryManager repo) : IUserService
    {
        private readonly IRepositoryManager Repository = repo;
        public List<User> GetAllUsers(bool trackChanges = false)
        {
            return this.Repository.UserRepository.GetAllUsers(trackChanges);
        }
        public User GetUser(int id)
        {
            return Repository.UserRepository.GetUser(id);
        }
        public List<User> GetUsersByCondition(Expression<Func<User, bool>> expression, bool trackChanges)
        {
            return Repository.UserRepository.GetUsersByCondition(expression, trackChanges);
        }
        public User? GetUserFromIdentity(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var userIdClaim = user.FindFirst("Id");
                if(userIdClaim != null)
                {
                    return GetUser(int.Parse(userIdClaim.Value));
                }
            }
            return null;
            
        }
        public string? GetRole(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var userRoleClaim = user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                if (userRoleClaim != null)
                {
                    return userRoleClaim.Value;
                }
            }
            return null;
        }
    }   
}
