using System.Collections.Generic;

namespace DataAccess.Model
{
  public class Menu : BaseEntity
  {
    public string Icon { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }

    public Menu()
    {
      Active = true;
    }

  }
}
