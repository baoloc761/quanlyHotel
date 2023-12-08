using NotificationLayer.Email.Models;
using System.Threading.Tasks;

namespace NotificationLayer
{
  public interface IEmailProvider
  {
    Task ExecuteAsync(EmailProviderParameter parameter);
    Task<EmailTemplates> GetTemplatesAsync();
    Task<EmailTemplate> GetTemplateByNaneAsync(string templateName);
  }
}
