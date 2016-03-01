using System.Collections.Generic;

namespace PasswordManager.Core.Interfaces
{
    public interface IEntityPersistence<T> 
    {
        bool PutList(List<T> entities);
        bool AddRange(List<T> entities);
        List<T> GetList();

        bool EnsureList();
    }
}