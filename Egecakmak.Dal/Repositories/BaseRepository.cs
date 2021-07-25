using Egecakmak.Utility;
using Egecakmak.Utility.Helper;
using Egecakmak.Utility.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Egecakmak.Dal.Repositories
{
  /// <summary>
  /// Generic Repository for data access on a single object
  /// </summary>
  /// <typeparam name="T">Databse Model Object</typeparam>
  /// <typeparam name="U">Primary Key Type (for single pkey fields)</typeparam>
  public class BaseRepository<T, U> : IDisposable, IBaseRepository<T, U> where T : class
  {
    public readonly DbContext dBContext;

    public BaseRepository(DbContext dBContext)
    {
      this.dBContext = dBContext;
    }

    /// <summary>
    /// Adds a the T into the DB Context
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Add(T entity)
    {
      dBContext.Set<T>().Add(entity);
    }

    /// <summary>
    /// Adds the multiple values in T into the DB Context
    /// </summary>
    /// <param name="entities"></param>
    public virtual void AddRange(IEnumerable<T> entities)
    {
      dBContext.Set<T>().AddRange(entities);
    }


    /// <summary>
    /// Disposes the DbContext
    /// </summary>
    public virtual void Dispose()
    {
      if (this.dBContext != null)
        this.dBContext.Dispose();
    }

    /// <summary>
    /// Set the T entity depending on the given predicate filter
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    {
      //if (typeof(T).GetProperties().Any(p => p.Name.ToLowerInvariant() == "isdeleted"))
      //{
      //	predicate = predicate.And()
      //}
      return dBContext.Set<T>().Where(predicate);
    }

    /// <summary>
    /// Updates a the T entity into the DB Context
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Update(T entity)
    {
      dBContext.Set<T>().Attach(entity);
      dBContext.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Updates given multiple T entities into the DB Context
    /// </summary>
    /// <param name="entity"></param>
    public virtual void UpdateRange(IEnumerable<T> entity)
    {
      dBContext.Set<T>().AttachRange(entity);
      dBContext.Entry(entity).State = EntityState.Modified;
    }


    /// <summary>
    /// Gets the T entity by provided id and loads into the DB Context
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual T Get(U id)
    {
      return dBContext.Set<T>().Find(id);
    }

    /// <summary>
    /// Gets all the requested data in the given T entity and loads into the DB Context
    /// </summary>
    /// <returns></returns>
    public virtual IQueryable<T> GetAll()
    {
      return dBContext.Set<T>();
    }

    /// <summary>
    /// Asynchronously gets one element that is filtered from the T
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public virtual T GetOne(
    Expression<Func<T, bool>> filter = null,
    string includeProperties = "")
    {
      return GetQueryable(filter, null, includeProperties).SingleOrDefault();
    }

    /// <summary>
    /// Gets one element that is filtered from the T
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public virtual async System.Threading.Tasks.Task<T> GetOneAsync(
        Expression<Func<T, bool>> filter = null,
        string includeProperties = null)
    {
      return await GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
    }

    /// <summary>
    /// Returns the first element of T
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public virtual T GetFirst(
       Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "")
    {
      return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously returns the first element of T
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public virtual async System.Threading.Tasks.Task<T> GetFirstAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null)
    {
      return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
    }


    /// <summary>
    /// Removes the elements from the T from the DB Context
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Remove(T entity)
    {
      dBContext.Set<T>().Remove(entity);
    }

    /// <summary>
    /// Removes a range of elements from the T from the DB Context
    /// </summary>
    /// <param name="entities"></param>
    public virtual void RemoveRange(IEnumerable<T> entities)
    {
      dBContext.Set<T>().RemoveRange(entities);
    }

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements in to the DB Context
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
      return dBContext.Set<T>().SingleOrDefault(predicate);
    }

    protected virtual IQueryable<T> GetQueryable(
    Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = null,
    int? skip = null,
    int? take = null)
    {
      includeProperties = includeProperties ?? string.Empty;
      IQueryable<T> query = dBContext.Set<T>();

      if (filter != null)
      {
        query = query.Where(filter);
      }

      foreach (var includeProperty in includeProperties.Split
          (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        query = query.Include(includeProperty);
      }

      if (orderBy != null)
      {
        query = orderBy(query);
      }

      if (skip.HasValue)
      {
        query = query.Skip(skip.Value);
      }

      if (take.HasValue)
      {
        query = query.Take(take.Value);
      }

      return query;
    }


    /// <summary>
    /// Gets the number of elements contained in the T
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual int GetCount(Expression<Func<T, bool>> filter = null)
    {
      return GetQueryable(filter).Count();
    }

    /// <summary>
    /// Asynchronously gets the number of elements contained in the T
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual System.Threading.Tasks.Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
    {
      return GetQueryable(filter).CountAsync();
    }

    /// <summary>
    /// Determines whether the contains elements that match the conditions defined by the specified predicate
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual bool GetExists(Expression<Func<T, bool>> filter = null)
    {
      return GetQueryable(filter).Any();
    }

    /// <summary>
    /// Asynchronously determines whether the contains elements that match the conditions defined by the specified predicate
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual System.Threading.Tasks.Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
    {
      return GetQueryable(filter).AnyAsync();
    }

    /// <summary>
    /// Asterius DB Context ile Sorgulama için (Fakat sadece primitive type için çalışır)
    /// </summary>
    /// <typeparam name="T">Table or View</typeparam>
    /// <typeparam name="X">Type of order by field</typeparam>
    /// <param name="pagesize"></param>
    /// <param name="pagenum"></param>
    /// <param name="totalrecords"></param>
    /// <param name="predicate">optional filter</param>
    /// <param name="orderby">optional order by</param>
    /// <param name="orderbyAscending">sort direction if exists</param>
    /// <returns></returns>
    public List<T> PagedGetAll<X>(int pagesize, int pagenum, out int totalrecords, Expression<Func<T, bool>> predicate = null, Expression<Func<T, X>> orderby = null, bool orderbyAscending = true)
    {
      try
      {
        var qry = dBContext.Set<T>().AsQueryable();
        if (predicate != null) qry = qry.Where(predicate);
        if (orderby != null)
        {
          if (orderbyAscending)
            qry = qry.OrderBy(orderby);
          else
            qry = qry.OrderByDescending(orderby);
        }
        totalrecords = qry.Count();
        return qry.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToList();
      }
      catch (Exception ex)
      {
        throw new MyException("[{0}] in [PagedGetAll] {1}", ex.GetType().Name, ex.AllMessages());
      }
    }

    /// <summary>
    /// Get Queryable Data With Filter and OrderBy
    /// </summary>
    /// <typeparam name="Y"></typeparam>
    /// <param name="filter"></param>
    /// <param name="orderby"></param>
    /// <param name="orderbyAscending"></param>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<Y>(Expression<Func<T, bool>> filter = null, Expression<Func<T, Y>> orderby = null, bool orderbyAscending = true)
    {
      try
      {
        var qry = dBContext.Set<T>().AsQueryable();
        if (filter != null) qry = qry.Where(filter);
        if (orderby != null)
        {
          if (orderbyAscending)
            qry = qry.OrderBy(orderby);
          else
            qry = qry.OrderByDescending(orderby);
        }

        return qry;
      }
      catch (Exception ex)
      {
        throw new MyException("[{0}] in [GetOrderedQuery] {1}", ex.GetType().Name, ex.AllMessages());
      }
    }

    //public IQueryable<T> GetQueryableOrderByString(Expression<Func<T, bool>> filter = null, string orderby = null, bool orderbyAscending = true)
    //{
    //  try
    //  {
    //    IQueryable<T> qry;
    //    qry = dBContext.Set<T>().AsQueryable();

    //    if (filter != null) qry = qry.Where(filter);
    //    if (orderby != null)
    //    {
    //      if (orderbyAscending)
    //        qry = qry.OrderBy(orderby);
    //      else
    //        qry = qry.OrderByDescending(orderby);
    //    }

    //    return qry;
    //  }
    //  catch (Exception ex)
    //  {
    //    throw new MyException("[{0}] in [GetOrderedQuery] {1}", ex.GetType().Name, ex.AllMessages());
    //  }
    //}

    public async Task<IEnumerable<T>> GetListAsync<Y>(Expression<Func<T, bool>> filter = null, Expression<Func<T, Y>> orderby = null, bool orderbyAscending = true)
    {
      try
      {
        var qry = dBContext.Set<T>().AsQueryable();
        if (filter != null) qry = qry.Where(filter);
        if (orderby != null)
        {
          if (orderbyAscending)
            qry = qry.OrderBy(orderby);
          else
            qry = qry.OrderByDescending(orderby);
        }

        return await qry.ToListAsync();
      }
      catch (Exception ex)
      {
        throw new MyException("[{0}] in [GetOrderedQuery] {1}", ex.GetType().Name, ex.AllMessages());
      }
    }

    public IQueryable<T> GetQueryableFromSql(string sql)
    {
      return dBContext.Set<T>().FromSqlRaw(sql);
    }
  }
}
