using Egecakmak.Domain.Models;
using Egecakmak.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egecakmak.Web.Controllers
{
  [Authorize]
  [Area("Admin")]
  public class PostController : BaseController
  {
    public PostController(IServiceProvider provider) : base(provider)
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
      return View(WebBaseManager.BllBaseManager.PostManager.GetAllPostQueryable());
    }

    public PartialViewResult GridView()
    {
      return PartialView("_IndexGrid", WebBaseManager.BllBaseManager.PostManager.GetAllPostQueryable());
    }

    public IActionResult TestPartial()
    {
      return PartialView("_IndexGrid", WebBaseManager.BllBaseManager.PostManager.GetAllPostQueryable());
    }

    public IActionResult CreatePost()
    {
      return View("_CreatePost");
    }


    #region [ Methods]

    public async Task<IActionResult> CreatePost(PostDto model)
    {
      if (ModelState.IsValid)
      {
        var result = await WebBaseManager.BllBaseManager.PostManager.Upsert(model);
        return RedirectToAction("Index");
      }
      return View("_CreatePost", model);
    }

    #endregion
  }
}
