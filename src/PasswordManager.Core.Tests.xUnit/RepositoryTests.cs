using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordManager.Core.Interfaces;
using Shouldly;
using Xunit;

namespace PasswordManager.Core.Tests.xUnit
{
    public class RepositoryTests : IClassFixture<StructureMapFixture>
    {
        private readonly StructureMapFixture _fixture;

        public RepositoryTests(StructureMapFixture fixture)
        {
            _fixture = fixture;
            _fixture.Container.GetInstance<IConfigurationSettings>().MasterKey = "Test1234";
        }

        [Fact]
        public void RepositoryEntityReferenceTest()
        {
            var repository = _fixture.Container.GetInstance<IRepository<PasswordEntity>>();
            repository.EnsureRepository();

            var passwordEntity = new PasswordEntity
            {
                CommonName = "Amazon",
                Url = "www.amazon.com",
                UserName = "dave@sample.com",
                Password = "pass@word1"
            };

            repository.Insert(passwordEntity).ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void UpdateShouldNotCreateNewEntryTest()
        {
            var repository = _fixture.Container.GetInstance<IRepository<PasswordEntity>>();
            repository.EnsureRepository();
            
            var passwordEntity = new PasswordEntity
            {
                CommonName = "Amazon",
                Url = "www.amazon.com",
                UserName = "dave@sample.com",
                Password = "pass@word1"
            };

            string id = repository.Insert(passwordEntity);
            var recordCount = repository.GetAll().Count();
            
            bool success = repository.Update(new PasswordEntity
            {
                Id = id,
                CommonName = "UpdateAmazon",
                Url = "Updatewww.amazon.com",
                UserName = "Updatedave@sample.com",
                Password = "Updatepass@word1"
            });

            success.ShouldBe(true);
            repository.GetAll().Count().ShouldBe(recordCount);

            var updateItem = repository.GetById(id, Cloner);
            updateItem.UserName.ShouldBe("Updatedave@sample.com");
        }

        [Fact]
        public void UpdateWithCopierShouldNotCreateNewEntryTest()
        {
            //Update
            var repository = _fixture.Container.GetInstance<IRepository<PasswordEntity>>();
            repository.EnsureRepository();
            
            var passwordEntity = new PasswordEntity
            {
                CommonName = "Amazon",
                Url = "www.amazon.com",
                UserName = "dave@sample.com",
                Password = "pass@word1"
            };

            string id = repository.Insert(passwordEntity);
            var recordCount = repository.GetAll().Count();

            var item = repository.Update(id, entity =>
            {
                entity.CommonName = "UpdateCommonName";
                entity.Password = "UpdatedPassword";
                entity.Url = "UpdatedUrl";
                entity.UserName = "UpdatedUserName";
            });

            item.ShouldNotBeNull();
            repository.GetAll().Count().ShouldBe(recordCount);

            var updateItem = repository.GetById(id, Cloner);
            updateItem.UserName.ShouldBe("UpdatedUserName");

        }

        private PasswordEntity Cloner(PasswordEntity passwordEntity)
        {
            return new PasswordEntity
            {
                Id = passwordEntity.Id,
                Password = passwordEntity.Password,
                UserName = passwordEntity.UserName,
                Url = passwordEntity.Url,
                CommonName = passwordEntity.CommonName
            };
        }


        [Fact]
        public void RepositoryContainerCloningReferenceTest()
        {   
            var repository = _fixture.Container.GetInstance<IRepository<PasswordEntity>>();

            repository.EnsureRepository();

            var passwordEntity = new PasswordEntity
            {
                CommonName = "Amazon",
                Url = "www.amazon.com",
                UserName = "dave@sample.com",
                Password = "pass@word1"
            };

            var item = repository.Insert(passwordEntity, entity => new PasswordEntity {
                                                                        CommonName = entity.CommonName,
                                                                        Id = entity.Id,
                                                                        Password = entity.Password,
                                                                        Url = entity.Url,
                                                                        UserName = entity.UserName});

            passwordEntity.ShouldNotBe(item);
        }
    }
}
