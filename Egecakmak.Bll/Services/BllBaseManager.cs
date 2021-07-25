using Egecakmak.Bll.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chryso.Bll.Services
{
  public class BllBaseManager
  {
    public BllBaseManager(
      PostManager postManager
      )
    {
      PostManager = postManager;
    }

    public PostManager PostManager { get; }
  }
}
