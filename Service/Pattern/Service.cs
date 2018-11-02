
using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Pattern
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        #region Private Fields
        
        IUnitOfWork utwk;
        #endregion Private Fields

        #region Constructor
        protected Service(IUnitOfWork utwk)
        {
            this.utwk = utwk;
        }
        #endregion Constructor



        public virtual void Add(TEntity entity)
        {
            ////_repository.Add(entity);
            utwk.getRepository<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            //_repository.Update(entity);
            utwk.getRepository<TEntity>().Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            //   _repository.Delete(entity);
            utwk.getRepository<TEntity>().Delete(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            // _repository.Delete(where);
            utwk.getRepository<TEntity>().Delete(where);
        }

        public virtual TEntity GetById(long id)
        {
            //  return _repository.GetById(id);
            return utwk.getRepository<TEntity>().GetById(id);
        }

        public virtual TEntity GetById(string id)
        {
            //return _repository.GetById(id);
            return utwk.getRepository<TEntity>().GetById(id);
        }

        public virtual TEntity GetById(int id)
        {
            //  return _repository.GetById(id);
            return utwk.getRepository<TEntity>().GetById(id);
        }

        public virtual IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, bool>> orderBy = null)
        {
            //  return _repository.GetAll();
            return utwk.getRepository<TEntity>().GetMany(filter,orderBy);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            //return _repository.Get(where);
            return utwk.getRepository<TEntity>().Get(where);
        }

        public virtual async Task<int> CountAsync()
        {
            //return await _repository.CountAsync();
            return await utwk.getRepository<TEntity>().CountAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            //return await _repository.GetAllAsync();
            return await utwk.getRepository<TEntity>().GetAllAsync();
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            //return await _repository.FindAsync(match);
            return await utwk.getRepository<TEntity>().FindAsync(match);
        }

        public virtual async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            // return await _repository.FindAllAsync(match);
            return await utwk.getRepository<TEntity>().FindAllAsync(match);
        }

       
       
        public void Commit()
        {
            try
            {
                utwk.Commit();
            }
            catch (Exception )
            {

              
            }
        }

        public void CommitAsync()
        {
            try
            {
                utwk.CommitAsync();
            }
            catch (Exception )
            {

            
            }
        }

        public void Dispose()
        {
            utwk.Dispose();
        }






        #region Singleton
        private static SqlConnection sqlConnection;
        protected static SqlConnection getConnectionInstance(string connection)
        {

            if (sqlConnection == null)
            {

                sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
            }
            else
            {
                sqlConnection.Close();
                sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
            }

            return sqlConnection;
        }


        #endregion
    }
}
