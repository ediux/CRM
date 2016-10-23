using My.Core.Infrastructures.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class OpenWebSiteEntities : IUnitofWork
    {
        private bool _requireuniqueEmail=false;
        public bool RequireUniqueEmail { get { return _requireuniqueEmail; } set { _requireuniqueEmail = value; } }

        private Hashtable _repositories;

        public OpenWebSiteEntities(string nameorconnectionstring)
            : base(nameorconnectionstring)
        {
            _requireuniqueEmail = false;
        }

        public static OpenWebSiteEntities Create()
        {
            return new OpenWebSiteEntities();
        }

      

        public System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class
        {
            return this.Entry<TEntity>(entity);
        }


        public TRepository GetRepository<TRepository, TEntity>()
            where TRepository : IRepositoryBase<TEntity>
            where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TRepository).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(TRepository);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                    , this);

                _repositories.Add(type, repositoryInstance);
            }

            return (TRepository)_repositories[type];
        }


        public IDbSet<TEntity> GetEntity<TEntity>() where TEntity : class
        {
           return base.Set<TEntity>();
        }
    }
}
