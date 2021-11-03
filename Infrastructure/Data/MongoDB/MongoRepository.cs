using Domain.Interfaces;
using MongoDB.Driver;
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

        public T Get(string id)
        {
            return _collection.Find<T>(item => item.Id == id).FirstOrDefault();
        }

        public void Create(T entity)
        {
            _collection.InsertOne(entity);
        }

        public void CreateAsync(T entity)
        {
            _collection.InsertOneAsync(entity);
        }

        public void Update(T entity)
        {
            if (string.IsNullOrEmpty(entity.Id)) { return; }

            _collection.ReplaceOne(item => item.Id == entity.Id, entity);
        }

        public void Remove(T entity)
        {
            _collection.DeleteOne(item => item.Id == entity.Id);
        }

        public void Remove(string id)
        {
            _collection.DeleteOne(item => item.Id == id);
        }
    }
}