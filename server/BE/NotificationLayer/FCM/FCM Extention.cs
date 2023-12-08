using Newtonsoft.Json;
using System.Collections.Generic;

namespace NotificationLayer.FCM
{
  public static class FCM_Extention
  {
    public static Dictionary<string, object> ToDictionary(this object source)
    {
      var json = JsonConvert.SerializeObject(source);
      var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
      return dictionary;
    }
  }
}
