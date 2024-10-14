using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Repository
{
    public interface IUserTypeRepository
    {
        List<UserType> GetAllUserTypes(bool trackChanges = false);
        UserType GetUserTypeById(int id);
    }
}
