using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAccess.Services.Interface
{
  public interface IAttachmentService
  {
    Task<List<Attachment>> GetDocumentsList(string keyword);
    Task<List<Attachment>> GetDocumentsList(List<Guid> ids);
    Task<Attachment> GetAttachmentById(Guid id);
    Task<Attachment> Update(Attachment attachment);
    Task Add(Attachment attachment);
    Task Delete(Guid fileId);
    Task<List<Attachment>> GetUserDocuments(Guid userId);
    Task<Attachment> GetUserAvatar(Guid userId);
    Task<UserIdentifyImages> GetUserIdentifyImages(Guid userId);
  }
}

public class UserIdentifyImages
{
  public Attachment BackImage { get; set; }
  public Attachment FrontImage { get; set; }
}
