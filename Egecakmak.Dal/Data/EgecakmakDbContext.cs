using Egecakmak.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egecakmak.Dal.Data
{
  public class EgecakmakDbContext : IdentityDbContext<EG_MEMBERS>
  {
    public EgecakmakDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TbPosts> TbPosts { get; set; }
  }
}
