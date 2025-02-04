﻿using InnoShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Contracts.Service
{
    public interface IUserTypeService
    {
        public List<UserType> GetAllUserTypes(bool trackChanges = false);
        public UserType GetUserType(int id);
    }
}
