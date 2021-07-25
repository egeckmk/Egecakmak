
using Egecakmak.Dal.Repositories;
using Egecakmak.Dal.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GM.NetStandard.DAL.UnitOfWork
{
  /// <summary>
  /// base unit for generating repositories and commiting changes
  /// </summary>
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DbContext context;

    public DbContext DbContext => context;

    public UnitOfWork(DbContext context)
    {
      this.context = context;
    }


    public void Dispose()
    {
      context.Dispose();
    }

    public IBaseRepository<T, U> GetRepository<T, U>() where T : class
    {
      return new BaseRepository<T, U>(context);
    }

    public int Save()
    {
      return context.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
      return await context.SaveChangesAsync();
    }
  }
}
