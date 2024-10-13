﻿using InnoShop.Contracts.Repository;
using InnoShop.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InnoShop.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T>(InnoShopContext context) : IRepositoryBase<T> where T : class
    {
        protected InnoShopContext Context = context;
        public IQueryable<T> FindAll(bool trackChanges = false) =>
        !trackChanges ? Context.Set<T>()
        .AsNoTracking() : Context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
            !trackChanges ? Context.Set<T>().Where(expression)
            .AsNoTracking() : Context.Set<T>().Where(expression);

        public void Create(T entity) => Context.Set<T>().Add(entity);
        public void Update(T entity) => Context.Set<T>().Update(entity);
        public void Delete(T entity) => Context.Set<T>().Remove(entity);

    }
}
