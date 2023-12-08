using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Model
{
  public class Post : BaseEntity
  {
    [MaxLength(200)]
    public string Title { get; set; }
    public string Content { get; set; }

    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }
  }

}
