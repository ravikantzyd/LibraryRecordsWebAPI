using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library_Records_Web_API.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            List<string> includes = null
            );

        Task<IList<T>> GetDataFromRawQuery(string query);

        Task SetDataFromRawQuery(string proc_name, List<SqlParameter> parameters);

        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);

        Task<int> GetCount(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            List<string> includes = null);

        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Delete(int id);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
