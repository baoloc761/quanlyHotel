using BusinessAccess.DataContract;
using System.Collections.Generic;

namespace BusinessAccess.Services.Interface
{
  public interface IBlogService
  {
    List<BlogContract> GetAllBlogs();
  }
}
