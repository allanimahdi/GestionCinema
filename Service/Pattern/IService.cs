using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Pattern
{
    public interface IService<T>:IDisposable
    where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T GetById(int id);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where=null, Expression<Func<T, bool>> orderBy=null);
        T Get(Expression<Func<T, bool>> where);
        Task<int> CountAsync();

        Task<List<T>> GetAllAsync();

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match);

        void Commit();
        void CommitAsync();

    }
    
}
