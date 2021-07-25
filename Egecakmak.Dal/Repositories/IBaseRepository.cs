using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Egecakmak.Dal.Repositories
{
  public interface IBaseRepository<T, U> where T : class
  {
    T Get(U id);
    IQueryable<T> GetAll();
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);

    T FirstOrDefault(Expression<Func<T, bool>> predicate);

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    int GetCount(Expression<Func<T, bool>> filter = null);
    System.Threading.Tasks.Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
    bool GetExists(Expression<Func<T, bool>> filter = null);
    System.Threading.Tasks.Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null);
    T GetFirst(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
    System.Threading.Tasks.Task<T> GetFirstAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);
    T GetOne(Expression<Func<T, bool>> filter = null, string includeProperties = "");
    System.Threading.Tasks.Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);

    IQueryable<T> GetQueryable<Y>(Expression<Func<T, bool>> filter = null, Expression<Func<T, Y>> orderby = null, bool orderbyAscending = true);
    //IQueryable<T> GetQueryableOrderByString(Expression<Func<T, bool>> filter = null, string orderby = null, bool orderbyAscending = true);
    List<T> PagedGetAll<X>(int pagesize, int pagenum, out int totalrecords, Expression<Func<T, bool>> predicate = null, Expression<Func<T, X>> orderby = null, bool orderbyAscending = true);

    System.Threading.Tasks.Task<IEnumerable<T>> GetListAsync<Y>(Expression<Func<T, bool>> filter = null, Expression<Func<T, Y>> orderby = null, bool orderbyAscending = true);

    IQueryable<T> GetQueryableFromSql(string sql);
  }
}
