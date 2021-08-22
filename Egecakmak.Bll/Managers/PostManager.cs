using Egecakmak.Dal.Repositories;
using Egecakmak.Domain.Entities;
using Egecakmak.Domain.Models;
using Egecakmak.Utility.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egecakmak.Portal.Bll.Managers;

namespace Egecakmak.Bll.Managers
{
  public class PostManager : BaseManager
  {
    private readonly IBaseRepository<TbPosts, int> _postRepository;

    public PostManager(IServiceProvider provider) : base(provider)
    {
      _postRepository = UnitOfWork.GetRepository<TbPosts, int>();
    }


    #region [... Post Methots ...]

    /// <summary>
    /// Create Or Update Post
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<PostDto> Upsert(PostDto model)
    {
      var newRecord = false;
      var entity = await _postRepository.GetFirstAsync(w => !w.IsDeleted && w.Id == model.Id);
      if (entity == null)
      {
        newRecord = true;
        entity = new TbPosts();
      }

      model.CopyToAnotherEntity(entity);

      if (newRecord) _postRepository.Add(entity);
      await UnitOfWork.SaveAsync();
      return entity.ToAnotherEntity<PostDto>();
    }

    /// <summary>
    /// Get Post By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PostDto> GetPostById(int id)
    {
      var entity = await _postRepository.GetFirstAsync(w => !w.IsDeleted && w.Id == id);
      if (entity == null) throw new Exception($"Post id {id} not found.");
      return entity.ToAnotherEntity<PostDto>();
    }

    /// <summary>
    /// Return all posts quaryable
    /// </summary>
    /// <returns></returns>
    public IQueryable<PostDto> GetAllPostQueryable()
    {
      var query = _postRepository.GetAll().Include(i => i.Member).Select(s => new PostDto
      {
        Id = s.Id,
        Title = s.Title,
        SubTitle = s.SubTitle,
        Content = s.Content,
        CreateTime = s.CreateTime,
        UpdateTime = s.UpdateTime,
        IsDeleted = s.IsDeleted,
        IsActive = s.IsActive,
        CreatedBy = s.CreatedBy,
        Member = s.Member
      }).AsQueryable();
      return query;
    }

    /// <summary>
    /// Delete Post By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeletePostById(int id)
    {
      
        var entity = await _postRepository.GetFirstAsync(w => !w.IsDeleted && w.Id == id);
        if (entity == null) throw new Exception($"Post id {id} not found.");
        entity.IsDeleted = true;
        await UnitOfWork.SaveAsync();
        return true;
    }
    #endregion
  }
}
