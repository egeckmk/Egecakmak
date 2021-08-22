using Egecakmak.Dal.UoW;
using System;

namespace Egecakmak.Portal.Bll.Managers
{
  public abstract class BaseManager
  {
    public BaseManager(IServiceProvider provider)
    {
      this.ServiceProvider = provider;
      this.UnitOfWork = (IUnitOfWork)provider.GetService(typeof(IUnitOfWork));
    }

    internal IServiceProvider ServiceProvider { get; }
    internal IUnitOfWork UnitOfWork { get; }
  }
}
