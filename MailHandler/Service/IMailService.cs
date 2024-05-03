using GroupCoursework.MailHandler.MailDTO;
using System.Threading.Tasks;

namespace GroupCoursework.MailHandler.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
