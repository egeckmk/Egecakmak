using Egecakmak.Bll.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egecakmak.Web.Services
{
  public static class WebServices
  {
    public static IServiceCollection AddWebBaseServices(this IServiceCollection services)
    {
      services.AddBllServices();
      services.AddTransient<WebBaseManager, WebBaseManager>();
      return services;
    }
  }
}
