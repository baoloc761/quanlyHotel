using System.Collections.Generic;

namespace NotificationLayer.Email.Models
{
  public class EmailTemplate
  {
    public string id { get; set; }
    public string name { get; set; }
    public string generation { get; set; }
    public List<Version> versions { get; set; }
  }
}
