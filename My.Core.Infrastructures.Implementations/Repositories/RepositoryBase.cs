using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;
using My.Core.Infrastructures.Implementations.Models;
using System.Data.Entity;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNet.Identity;
namespace My.Core.Infrastructures.Implementations
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected IUnitofWork _unitofwork;

        protected DbSet<TEntity> _datatable;

        public RepositoryBase(IUnitofWork unitofwork)
        {
            _unitofwork = unitofwork;
            _datatable = (DbSet<TEntity>)_unitofwork.GetEntity<TEntity>();
        }

        #region Helper Functions

        /// <summary>
        /// Writes the error log.
        /// </summary>
        /// <returns>The error log.</returns>
        /// <param name="ex">Ex.</param>
        protected virtual void WriteErrorLog(Exception ex)
        {
            if (_logger != null)
            {
                _logger.ErrorFormat("{0},{1}", ex.Message, ex.StackTrace);
            }
            else
            {
                System.Diagnostics.Debug.Write(string.Format("{0},{1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region Logger 記錄器
        private ILogWriter _logger;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ILogWriter Logger
        {
            get
            {
                return _logger;
            }

            set
            {
                _logger = value;
            }
        }
        #endregion

        protected virtual int GetCurrentLoginedUserId()
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            }

            return -1;
        }
        public virtual TEntity Create(TEntity entity)
        {
            try
            {
                var state = _unitofwork.GetEntry<TEntity>(entity);
                state.State = EntityState.Added;
                TEntity inserteduser = _datatable.Add((TEntity)entity);
                SaveChanges();
                return inserteduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public virtual IList<TEntity> BatchCreate(IEnumerable<TEntity> entities)
        {
            try
            {
                IList<TEntity> result = ((DbSet<TEntity>)_datatable).AddRange(entities).ToList();
                SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public virtual TEntity Find(params object[] values)
        {
            TEntity _currentexecutinguser = null;

            try
            {
                _currentexecutinguser = _datatable.Find(values);
                return _currentexecutinguser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public virtual IQueryable<TEntity> FindAll()
        {
            try
            {
                return _datatable;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                var state = _unitofwork.GetEntry<TEntity>(entity);
                state.State = EntityState.Modified;
                SaveChanges();
                state.Reload();
                entity= state.Entity;
                return entity;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public virtual void Delete(TEntity entity)
        {
            try
            {
                var deletedata = _unitofwork.GetEntry<TEntity>(entity);

                if (deletedata.State == EntityState.Detached)
                    deletedata.State = EntityState.Unchanged;

                _datatable.Remove(entity);
                SaveChanges();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }


        public virtual void SaveChanges()
        {
            _unitofwork.SaveChanges();
        }

        public IList<TEntity> ToList(IQueryable<TEntity> source)
        {
            try
            {
                return source.ToList();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    SaveChanges();
                    _unitofwork.Dispose();
                }
            }

            disposed = true;

        }


        public IList<TEntity> BatchUpdate(IEnumerable<TEntity> updatedEntities)
        {
            throw new NotImplementedException();
        }
    }
}
