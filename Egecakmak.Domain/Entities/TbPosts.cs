using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Egecakmak.Domain.Entities
{
  public class TbPosts
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string SubTitle { get; set; }

    public string Content { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; }

    #region [... Relations ...]

    [ForeignKey("CreatedBy")]
    public EG_MEMBERS Member { get; set; }
    
    #endregion
  }

}
