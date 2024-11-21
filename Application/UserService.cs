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
        public User? Authorize(string? fullToken)
        {
            string token = "null";
            if (fullToken != null)
                token = fullToken.ToString().Replace("Bearer ", "");
            if (token != "null")
            {
                // Проверяем, что токен не пустой
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value);
                return GetUser(userId);
            }
            else
                return null;
        }
        public string? GetRole(string? fullToken)
        {
            string token = "null";
            if (fullToken != null)
                token = fullToken.ToString().Replace("Bearer ", "");
            if (token != "null")
            {
                // Проверяем, что токен не пустой
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                var role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
                return role;
            }
            else
                return null;
        }
    }   
}
