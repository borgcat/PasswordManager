﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace PasswordManager.Core.Interfaces
{
    public interface IRepository<T> where T : IEntity 
    {
        void Delete(T agregateRoot);
        IQueryable<T> GetAll();
        T GetById(string id);
        T Insert(T agregateRoot);
        T Update(T agregateRoot);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        bool EnsureRepository();
    }
}