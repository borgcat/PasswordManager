using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Core.Repository
{
    public class EntityRepository<T> : IKey, IRepository<T> where T : IEntity
    {
        private readonly IConfigurationSettings _configuration;
        private readonly IEntityPersistence<T> _persistence;
        private readonly IPasswordManagement _passwordManagement;

        public string MasterKey
        {
            set { _configuration.EncryptedMasterKey = value; } 
        }

        public EntityRepository(IConfigurationSettings configuration, IEntityPersistence<T> persistence, IPasswordManagement passwordManagement)
        {
            _configuration = configuration;
            _persistence = persistence;
            _passwordManagement = passwordManagement;
        }

        public bool Delete(T aggregateRoot)
        {
            var records = _persistence.GetList();

            bool success = records.Remove(records.First(x=>x.Id == aggregateRoot.Id));
            _persistence.PutList(records);

            return success;
        }

        public bool Delete(string id)
        {
            return Delete(GetById(id));
        }

        public IQueryable<T> GetAll()
        {
            return _persistence.GetList().AsQueryable();
        }

        public T GetById(string id)
        {
            var records = _persistence.GetList();
            return records.First(w => w.Id == id.ToString());
        }

        public T GetById(string id, Func<T, T> cloner)
        {
           return cloner(GetById(id));
        }

        private T InternalInsert(T aggregateRoot)
        {
            aggregateRoot.Id = _passwordManagement.GeneratePassword();

            var records = _persistence.GetList();
            records.Add(aggregateRoot);

            if (_persistence.PutList(records))
                return aggregateRoot;

            return default(T);
        }

        public string Insert(T aggregateRoot)
        {
            return InternalInsert(aggregateRoot).Id;
        }

        public T Insert(T aggregateRoot, Func<T, T> cloner)
        {
            var item = Insert(aggregateRoot);
            if (item != null)
                return cloner(aggregateRoot);

            return default(T);
        }

        public bool Update(T aggregateRoot)
        {
            return UpdateInternalWithRemove(aggregateRoot) != null;
        }

        public T Update(string id, Action<T> cloner)
        {
            var records = _persistence.GetList();
            var item = records.First(w => w.Id == id);
            string persistId = item.Id;

            cloner(item);

            item.Id = persistId;

            if (_persistence.PutList(records))
                return item;

            return default(T);
        }

        private T UpdateInternalWithRemove(T aggregateRoot)
        {
            var records = _persistence.GetList();

            if (records.Count == 0)
                throw new Exception("Trying to update a record with an empty repository");

            T item = records.First(w => w.Id == aggregateRoot.Id);

            if (item != null)
            {
                records.Remove(item);
                records.Add(aggregateRoot);
            }

            if (_persistence.PutList(records))
                return aggregateRoot;

            return default(T);
        }

        public bool EnsureRepository()
        {
            return _persistence.EnsureList();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return _persistence.GetList().AsQueryable().Where(predicate);
        }
    }
}
