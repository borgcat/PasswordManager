using System.Collections.Generic;

namespace PasswordManager.Core.Interfaces
{
    public interface ISerialization<T>
    {
        List<T> DeserializeObject();
        bool SerializeObject(object o);
        bool EnsureContainer();
    }
}