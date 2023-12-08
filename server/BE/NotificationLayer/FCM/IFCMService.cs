using NotificationLayer.FCM;
using System.Threading.Tasks;

namespace NotificationLayer
{
  public interface IFCMService
  {
    Task SendNotification(NotificationParameter parameter);
  }
}
