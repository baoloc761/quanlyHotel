using System;
using System.Collections.Generic;

namespace DataAccess.Model
{
  public class UserAttachment : BaseEntity
  {
    public Guid UserId { get; set; }
    public Guid FileId { get; set; }
  }
}
