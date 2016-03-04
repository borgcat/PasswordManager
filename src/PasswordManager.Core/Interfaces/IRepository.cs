using System;
using System.Linq;
using System.Linq.Expressions;

namespace PasswordManager.Core.Interfaces
{
    public interface IRepository<T> where T : IEntity 
    {
        IQueryable<T> GetAll();

        bool Delete(T aggregateRoot);
        bool Delete(string id);

        T GetById(string id, Func<T, T> cloner);

        string Insert(T aggregateRoot);
        T Insert(T aggregateRoot, Func<T, T> cloner);

        bool Update(T aggregateRoot);
        T Update(string id, Action<T> cloner);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        bool EnsureRepository();
    }
}