using DataAccess.Helper;
using System;

namespace BusinessAccess.DataContract
{
  public class BlogContract
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public DateTime? CreatedTime { get; set; }

    public BlogContract()
    {
      Id = GuidComb.Generate();
    }
  }
}
