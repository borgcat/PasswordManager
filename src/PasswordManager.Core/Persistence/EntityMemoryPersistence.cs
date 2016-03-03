using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Persistence
{
    public class EntityMemoryPersistence<T> : IEntityPersistence<T>
    {
        private readonly List<T> _records = new List<T>();
        public bool PutList(List<T> entities)
        {
            // ignoring encription for memory implementation

            _records.Clear();
            _records.AddRange(entities);

            return true;
        }

        public bool AddRange(List<T> entities)
        {
            // ignoring encription for memory implementation

            _records.AddRange(entities);

            return true;
        }

        public List<T> GetList()
        {
            // ignoring encription for memory implementation

            return _records.ToList();
        }

        public bool EnsureList()
        {
            return _records != null;
        }
    } 
}