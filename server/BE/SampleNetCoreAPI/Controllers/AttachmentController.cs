using Microsoft.AspNetCore.Mvc;
using System;
using BusinessAccess.Services.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using SampleNetCoreAPI.Models;
using DataAccess.Model;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SampleNetCoreAPI.Helper;

namespace SampleNetCoreAPI.Controllers
{
  [Produces("application/json")]
  [Route("api/docs/")]
  [ApiExplorerSettings(GroupName = "others")]
  public class AttachmentController : Controller
  {
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
      _attachmentService = attachmentService;
    }

    [HttpGet, Route("get-user-avatar")]
    public async Task<IActionResult> GetUserAvatar([FromQuery] Guid userId)
    {
      try
      {
        var avatar = await _attachmentService.GetUserAvatar(userId);
        return Ok(new
        {
          status = 200,
          message = "Success",
          data = avatar
        });
      }
      catch
      {
        return Ok(new HotelResponse<Attachment>()
        {
          status = 404,
          message = "Failed"
        });
      }
    }

    [HttpPost, Route("get-docs-from-guid-list")]
    public async Task<IActionResult> GetDocumentsFromListGuid([FromBody] List<Guid> list)
    {
      try
      {
        var documentsList = await _attachmentService.GetDocumentsList(list);
        return Ok(new
        {
          status = 200,
          message = "Success",
          data = documentsList
        });
      }
      catch
      {
        return Ok(new
        {
          status = 404,
          message = "Failed"
        });
      }
    }

    [HttpGet, Route("get-docs-list")]
    public async Task<IActionResult> GetDocumentsList([FromQuery] string keyword = null)
    {
      try
      {
        var documentsList = await _attachmentService.GetDocumentsList(keyword);
        return Ok(new
        {
          status = 200,
          message = "Success",
          data = documentsList
        });
      }
      catch
      {
        return Ok(new
        {
          status = 404,
          message = "Failed"
        });
      }
    }
  }
}
