using Egecakmak.Dal.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egecakmak.Dal.UoW
{
  public interface IUnitOfWork : IDisposable
  {
    IBaseRepository<T, U> GetRepository<T, U>() where T : class;
    int Save();
    Task<int> SaveAsync();
    DbContext DbContext { get; }
  }
}
