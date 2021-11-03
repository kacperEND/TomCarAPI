using Domain.Interfaces;
using MongoDB.Driver;

namespace Domain.Interfaces
{
    public interface IMongoRepository<T> where T : ICoreAuditableMongoModel
    {
        IMongoCollection<T> Collection { get; }

        void Create(T entity);

        void CreateAsync(T entity);

        /// <summary>
        /// Gets the entity for the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(string id);

        void Remove(T entity);

        void Update(T entity);
    }
}