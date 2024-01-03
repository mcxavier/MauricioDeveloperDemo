using System.Threading.Tasks;

using Infra.ExternalServices.MailSender.Dtos;

namespace Infra.ExternalServices.MailSender
{

    public interface IEmailServices
    {

        Task<bool> SendEmailMessage(EmailMessage message);

    }

}