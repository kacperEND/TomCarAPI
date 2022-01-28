using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Domain.Interfaces
{
    public interface IMongoRepository<T> where T : ICoreAuditableMongoModel
    {
        IMongoCollection<T> Collection { get; }
        IMongoQueryable<T> QueryCollection { get; }

        void Create(T entity);

        /// <summary>
        /// Gets the entity for the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(string id);

        void DeleteAsync(T entity);

        void DeleteAsync(string id);

        void UpdateAsync(T entity);
    }
}