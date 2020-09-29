using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PVCBasic.Database.Repositories
{
    public class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
      where TEntity : class
      where TContext : DbContext
    {
        public RepositoryBase(TContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the context used in this repository.
        /// </summary>
        public virtual TContext Context { get; protected set; }

        /// <summary>
        /// When is overriden in a derived type, returns all the elements in the entity set.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable"/> for all the elements in the entity set.
        /// </returns>
        public virtual IQueryable<TEntity> All()
        {
            return this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Filters an entity set of values based on a predicate.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each element for a condition.
        /// </param>
        /// <returns>
        /// An <see cref="IQueryable"/> that contains elements from the input sequence that satisfy the condition specified by predicate.
        /// </returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return this.All().Where(predicate);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <param name="predicate">
        /// A sequence of values to project.
        /// </param>
        /// <typeparam name="TResult">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IQueryable"/> whose elements are the result of invoking a projection function on each element of source.
        /// </returns>
        public virtual IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> predicate)
        {
            return this.All().Select(predicate);
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        /// <param name="predicate">
        /// An expression to return the first element of a condition.
        /// </param>
        /// <returns>
        /// The first element that satisfies the condition.
        /// </returns>
        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Filter(predicate).First();
        }

        public virtual TEntity Find(params object[] keys)
        {
            return ((DbSet<TEntity>)this.All()).Find(keys);
        }

        public virtual ValueTask<TEntity> FindAsync(params object[] keys)
        {
            return this.FindAsync(CancellationToken.None, keys);
        }

        public virtual ValueTask<TEntity> FindAsync(CancellationToken token, params object[] keys)
        {
            return ((DbSet<TEntity>)this.All()).FindAsync(keys, token);
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        /// <param name="predicate">
        /// An expression to return the first element of a condition.
        /// </param>
        /// <returns>
        /// The first element that satisfies the condition.
        /// </returns>
        public virtual Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Filter(predicate).FirstAsync();
        }

        public TEntity Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks the entity as added.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The entity with its state marked as added.
        /// </returns>
        public virtual TEntity Create(TEntity entity)
        {
            this.Context.Add(entity);
            return entity;
        }

        /// <summary>
        /// Marks the entity as modified.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The entity with its state marked as modified.
        /// </returns>
        public virtual TEntity Update(TEntity entity)
        {
            var entry = this.Context.Update(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

       



        /// <summary>
        /// Marks the entity as deleted.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The entity with its state marked as deleted.
        /// </returns>
        public virtual TEntity Delete(TEntity entity)
        {
            var entry = this.Context.Remove(entity);
            entry.State = EntityState.Deleted;
            return entity;
        }

        public Task<int> SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The amount of changed records.</returns>
        public virtual int SaveChanges()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the first element of the entity set with the specified condition, or a default value if the entity set contains no elements.
        /// </summary>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <returns>
        /// default value, if the source is empty; otherwise, the first element in source.
        /// </returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.All().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first element of the entity set with the specified condition, or a default value if the entity set contains no elements.
        /// </summary>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <returns>
        /// default value, if the source is empty; otherwise, the first element in source.
        /// </returns>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.All().Where(predicate).FirstOrDefaultAsync();
        }
    }
}
