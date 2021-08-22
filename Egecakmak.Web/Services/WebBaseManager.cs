using Egecakmak.Bll.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egecakmak.Web.Services
{
  public class WebBaseManager
  {
    public WebBaseManager(BllBaseManager bllBaseManager)
    {
      BllBaseManager = bllBaseManager;
    }

    public BllBaseManager BllBaseManager { get; }
  }
}

