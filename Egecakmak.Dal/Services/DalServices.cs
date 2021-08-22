using Egecakmak.Dal.Data;
using Egecakmak.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Egecakmak.Dal.UoW;
using Microsoft.AspNetCore.Builder;

namespace Egecakmak.Dal.Services
{
  public static class DalServices
  {
    public static IServiceCollection AddDalServices(this IServiceCollection services)
    {
      var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
      services.AddDbContext<EgecakmakDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("EgeCakmakContext"), sqlServerOptions => sqlServerOptions.CommandTimeout(90)).EnableSensitiveDataLogging(true));

      services.AddScoped<Microsoft.EntityFrameworkCore.DbContext, EgecakmakDbContext>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();


      return services;
    }


    public static void UpdateEgecakmakDatabase(this IApplicationBuilder app)
    {
      using var serviceScope = app.ApplicationServices
          .GetRequiredService<IServiceScopeFactory>()
          .CreateScope();
      using var context = serviceScope.ServiceProvider.GetService<EgecakmakDbContext>();
      context.Database.Migrate();

    }
  }
}
