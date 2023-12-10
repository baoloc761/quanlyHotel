using DataAccess.Helper;
using System;

namespace DataAccess.Model
{
  public class BaseEntity
  {
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public DateTime? CreatedTime { get; set; }
    public BaseEntity()
    {
      Id = GuidComb.Generate();
      CreatedTime= DateTime.UtcNow;
      UpdatedTime= DateTime.UtcNow;
      Active = true;
    }
  }
}
