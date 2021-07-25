using Egecakmak.Dal.UnitOfWork;
using System;

namespace UMA.Portal.Bll.Managers
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
