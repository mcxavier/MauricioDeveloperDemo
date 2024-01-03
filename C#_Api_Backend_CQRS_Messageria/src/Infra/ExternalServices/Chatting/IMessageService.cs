using System.Threading.Tasks;

namespace Infra.ExternalServices.Chatting
{

    public interface IMessageService
    {

        Task<string> Send(string from, string to, string message);

    }

}