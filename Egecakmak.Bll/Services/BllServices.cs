using Egecakmak.Bll.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Egecakmak.Bll.Services
{
  public static class BllServices
  {

    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
      var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

      services.AddTransient<PostManager, PostManager>();

      services.AddTransient<BllBaseManager, BllBaseManager>();

      return services;
    }
  }
}
