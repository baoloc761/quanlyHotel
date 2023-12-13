using System.Collections.Generic;

namespace SampleNetCoreAPI.Models
{
  public class HotelResponse<T> where T : class
  {
    public int status { get; set; }
    public string message { get; set; }
    public string error { get; set; }
    public string reason { get; set; }
    public T data { get; set; }
    public List<T> dataList { get; set; }
    public HotelResponse()
    {
      dataList = new List<T>();
    }
  }
}
