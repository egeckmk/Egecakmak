using Egecakmak.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
namespace Egecakmak.Dal.Extentions
{
  public static class ServiceCollectionExtentions
  {
    public static IServiceCollection AddDalServices(this IServiceCollection services)
    {
      var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

      services.AddDbContext<EgecakmakDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("EgeCakmakContext")));

      return services;
    }
  }
}
