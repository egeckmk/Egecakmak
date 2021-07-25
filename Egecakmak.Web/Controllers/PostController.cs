using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egecakmak.Web.Controllers
{
  public class PostController : Controller
  {
   public IActionResult Posts()
    {
      return View();
    }
  }
}
