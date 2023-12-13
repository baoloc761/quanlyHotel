using System.Collections.Generic;

namespace DataAccess.Model
{
  public class AttachmentGroup : BaseEntity
  {
    public string GroupName { get; set; }
    public string Description { get; set; }
    public Attachment Attachment { get; set; }

  }
}
