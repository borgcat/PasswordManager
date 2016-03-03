using System.Collections.Generic;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Persistence
{
    public class JsonFilePersistence<T> : IEntityPersistence<T>
    {
        private readonly ISerialization<T> _serialization;
        
        public JsonFilePersistence(ISerialization<T> serialization)
        {
            _serialization = serialization;
        }
        
        public bool PutList(List<T> entities)
        {
            return _serialization.SerializeObject(entities);
        }

        public bool AddRange(List<T> entities)
        {
            var list = GetList();
            list.AddRange(entities);
            return PutList(list);
        }

        public List<T> GetList()
        {
            return _serialization.DeserializeObject();
        }

        public bool EnsureList()
        {
            return _serialization.EnsureContainer();
        }
    }
}