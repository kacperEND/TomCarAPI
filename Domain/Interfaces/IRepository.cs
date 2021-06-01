using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of an entity for the given type. The instance returned will be a proxy if the underlying context is configured to create proxies and the entity type meets the requirements for creating a proxy.
        /// </summary>
        /// <returns></returns>
        T New();

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the specified entity in the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(object id);

        /// <summary>
        /// Copies the specified source to the target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        void Copy(T source, T target);

        /// <summary>
        /// Commits all the pending changes to the database.
        /// </summary>
        void Flush();

        /// <summary>
        /// Gets the entity for the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T Get(object id);

        /// <summary>
        /// Gets the entity based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Queries the table and fetches all the data.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Get the record count based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Fetches the records based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Fetches the records based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="skip">Number of rows to skip.</param>
        /// <param name="count">Number of rows to fecth.</param>
        /// <returns></returns>
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate, int skip, int count);

        /// <summary>
        /// Gets a reference to the context's database
        /// </summary>
        /// <returns></returns>
        Database GetDatabase();

        /// <summary>
        /// Gets a reference to the context
        /// </summary>
        /// <returns></returns>
        DbContext GetContext();
    }
}