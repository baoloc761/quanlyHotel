using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SampleNetCoreAPI.Helper
{
  public static class ObjectUtils
  {
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
      var someObject = new T();
      var someObjectType = someObject.GetType();

      foreach (var item in source)
      {
        var key = char.ToUpper(item.Key[0]) + item.Key.Substring(1);
        var targetProperty = someObjectType.GetProperty(key);

        //edited this line
        if (targetProperty.PropertyType == item.Value.GetType())
        {
          targetProperty.SetValue(someObject, item.Value);
        }
        else
        {

          var parseMethod = targetProperty.PropertyType.GetMethod("TryParse",
              BindingFlags.Public | BindingFlags.Static, null,
              new[] { typeof(string), targetProperty.PropertyType.MakeByRefType() }, null);

          if (parseMethod != null)
          {
            var parameters = new[] { item.Value, null };
            var success = (bool)parseMethod.Invoke(null, parameters);
            if (success)
            {
              targetProperty.SetValue(someObject, parameters[1]);
            }

          }
        }
      }

      return someObject;
    }

    public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
      IDictionary<string, object> res = new Dictionary<string, object>();
      PropertyInfo[] props = source.GetType().GetProperties();
      for (int i = 0; i < props.Length; i++)
      {
        if (props[i].CanRead)
        {
          res.Add(props[i].Name, props[i].GetValue(source, null));
        }
      }
      return res;
    }
  }
}
