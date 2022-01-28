using Domain.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Infrastructure.Data.MongoDB
{
    public class MongoRepository<T> : IMongoRepository<T> where T : ICoreAuditableMongoModel
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDBDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var _database = client.GetDatabase(settings.DatabaseName);

            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        public IMongoCollection<T> Collection
        {
            get
            {
                return _collection;
            }
        }

        public IMongoQueryable<T> QueryCollection
        {
            get
            {
                return _collection.AsQueryable();
            }
        }

        public T Get(string id)
        {
            return _collection.Find<T>(item => item.Id == id).FirstOrDefault();
        }

        public void Create(T entity)
        {
            _collection.InsertOneAsync(entity);
        }

        public void UpdateAsync(T entity)
        {
            if (string.IsNullOrEmpty(entity.Id)) { return; }

            _collection.ReplaceOneAsync(item => item.Id == entity.Id, entity);
        }

        public void DeleteAsync(T entity)
        {
            _collection.DeleteOneAsync(item => item.Id == entity.Id);
        }

        public void DeleteAsync(string id)
        {
            _collection.DeleteOneAsync(item => item.Id == id);
        }
    }
}