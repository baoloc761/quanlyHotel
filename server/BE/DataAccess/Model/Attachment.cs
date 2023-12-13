using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Model
{
  public class Attachment : BaseEntity
  {
    public Guid FileGroupId { get; set; }
    public string Title { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string Src { get; set; }
    public string FileExtension { get; set; }
    public virtual AttachmentGroup AttachmentGroup { get; set; }
  }
}
