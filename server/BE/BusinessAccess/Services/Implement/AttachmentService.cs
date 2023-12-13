using AutoMapper;
using BusinessAccess.Repository;
using BusinessAccess.Services.Interface;
using DataAccess.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attachment = DataAccess.Model.Attachment;

namespace BusinessAccess.Services.Implement
{
  public class AttachmentService : IAttachmentService
  {
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AttachmentGroup> _attachmentGroupRepository;
    private readonly IRepository<Attachment> _attachmentRepository;
    private readonly IRepository<UserAttachment> _userAttachmentRepository;
    private readonly IMapper _mapper;
    private readonly ILog _logger;

    public AttachmentService(IRepository<User> userRepository, IRepository<AttachmentGroup> attachmentGroupRepository,
        IRepository<Attachment> attachmentRepository, IRepository<UserAttachment> userAttachmentRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _attachmentGroupRepository = attachmentGroupRepository;
      _attachmentRepository = attachmentRepository;
      _userAttachmentRepository = userAttachmentRepository;
      _logger = LogManager.GetLogger(typeof(UserService));
      _mapper = mapper;
    }

    public async Task<List<Attachment>> GetDocumentsList(List<Guid> ids)
    {
      try
      {
        var query = _attachmentRepository.Filter(x => x.Active && ids.Contains(x.Id)).AsQueryable();
        return await query.ToListAsync();
      }
      catch
      {
        return new List<Attachment>();
      }
    }

    public async Task<List<Attachment>> GetDocumentsList(string keyword)
    {
      try
      {
        var query = _attachmentRepository.Filter(x => x.Active).AsQueryable();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
          query = query
            .Where(x => x.AttachmentGroup.GroupName.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.AttachmentGroup.Description.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.FileExtension.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.Title.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.ContentType.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.Src.ToLower().IndexOf(keyword.ToLower()) >= 0 ||
             x.FileName.ToLower().IndexOf(keyword.ToLower()) >= 0)
            .AsQueryable();
        }
        return await query.ToListAsync();
      }
      catch
      {
        return new List<Attachment>();
      }
    }

    public async Task Add(Attachment attachment)
    {
      await _attachmentRepository.InsertAsync(attachment);
    }

    public async Task Delete(Guid fileId)
    {
      Attachment file = await _attachmentRepository.GetAsync(fileId);
      if (file == null)
      {
        throw new Exception("File not existed");
      }
      await _attachmentRepository.DeleteAsync(file);
    }

    public async Task<Attachment> GetAttachmentById(Guid id)
    {
      Attachment file = await _attachmentRepository.GetAsync(id);
      if (file == null)
      {
        throw new Exception("File not existed");
      }
      return file;
    }

    public async Task<Attachment> GetUserAvatar(Guid userId)
    {
      try
      {
        var user = await _userRepository.GetAsync(userId);
        if (user == null || user.AvatarFileId == null)
        {
          return null;
        }
        var avatar = await _attachmentRepository.Filter(x => x.Active && x.Id == user.AvatarFileId).FirstOrDefaultAsync();
        return avatar;
      }
      catch (Exception)
      {
        return null;
      }
    }

    public async Task<List<Attachment>> GetUserDocuments(Guid userId)
    {

      try
      {
        var attachmentList = await (
            from a in _attachmentRepository.Filter(x => x.Active)
            join b in _userAttachmentRepository.Filter(x => x.Active && x.UserId == userId) on a.Id equals b.FileId
            select a
          ).ToListAsync();
        return attachmentList;
      }
      catch (Exception)
      {
        return new List<Attachment>();
      }
    }

    public async Task<UserIdentifyImages> GetUserIdentifyImages(Guid userId)
    {
      try
      {
        var user = await _userRepository.GetAsync(userId);
        if (user == null || user.AvatarFileId == null)
        {
          return null;
        }
        var backImage = await _attachmentRepository.Filter(x => x.Active && x.Id == user.IdBackFileId).FirstOrDefaultAsync();
        var frontImage = await _attachmentRepository.Filter(x => x.Active && x.Id == user.IdFrontFileId).FirstOrDefaultAsync();
        return new UserIdentifyImages()
        {
          BackImage = backImage,
          FrontImage = frontImage
        };
      }
      catch (Exception)
      {
        return null;
      }
    }

    public async Task<Attachment> Update(Attachment attachment)
    {
      await _attachmentRepository.UpdateAsync(attachment);
      return attachment;
    }
  }
}
