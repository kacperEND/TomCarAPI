using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class EFRepository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly SqlServerDBContext _dbContext;

        public EFRepository(SqlServerDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<T> Table
        {
            get
            {
                return this.DbSet;
            }
        }

        public void Copy(T source, T target)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(T entity)
        {
            this.DbSet.Add(entity);
            var dbEntity = this._dbContext.Entry(entity);
            dbEntity.State = EntityState.Added;
        }

        public void Delete(object id)
        {
            var itemToRemove = this.Get(id);
            this.DbSet.Remove(itemToRemove);
        }

        public IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate, int skip, int count)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            this._dbContext.SaveChanges();
        }

        public T Get(object id)
        {
            return this.DbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Table.SingleOrDefault(predicate);
        }

        public DbContext GetContext()
        {
            throw new NotImplementedException();
        }

        public Database GetDatabase()
        {
            throw new NotImplementedException();
        }

        public T New()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            //if (entity.IsTransient())
            //{
            //    return;
            //}
            var dbEntity = this._dbContext.Entry(entity);
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }

        Database IRepository<T>.GetDatabase()
        {
            throw new NotImplementedException();
        }

        DbContext IRepository<T>.GetContext()
        {
            throw new NotImplementedException();
        }

        private DbSet<T> DbSet
        {
            get
            {
                return this._dbContext.Set<T>();
            }
        }
    }
}