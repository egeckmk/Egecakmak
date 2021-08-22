using Egecakmak.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egecakmak.Web.Controllers
{
  public class BaseController : Controller
  {
    public IServiceProvider ServiceProvider { get; }
    public WebBaseManager WebBaseManager { get; }
    public BaseController(IServiceProvider provider)
    {
      ServiceProvider = provider;
      WebBaseManager = ServiceProvider.GetRequiredService<WebBaseManager>();
    }
  }
}
