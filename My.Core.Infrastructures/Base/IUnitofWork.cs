using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace My.Core.Infrastructures.DAL
{
    /// <summary>
    /// Unitof work.
    /// </summary>
    public interface IUnitofWork : IDisposable
    {

        DbEntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        IDbSet<TEntity> GetEntity<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <returns>The repository.</returns>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        TRepository GetRepository<TRepository, TEntity>()
            where TRepository : IRepositoryBase<TEntity>
            where TEntity : class;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>The changes.</returns>
        int SaveChanges();

    }

    public interface IUnitofWork<TDbContext> : IUnitofWork
        where TDbContext : DbContext
    {
        /// <summary>
        /// 取得資料庫物件執行個體。
        /// </summary>
        /// <returns>The database object.</returns>
        /// <typeparam name="TDbContext">The 1st type parameter.</typeparam>
        TDbContext GetDatabaseObject<TDbContext>();
    }
}

