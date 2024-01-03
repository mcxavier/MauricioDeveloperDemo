using Api.ViewModels;
using Infra.ExternalServices.Chatting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Infra.EntitityConfigurations.Contexts;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Api.Controllers
{

    [Route("api/v1/[controller]"), ApiController]
    public class WhatsappController : TwilioController
    {

        private readonly IMessageService _messageService;
        private readonly CoreContext _context;

        public WhatsappController(IMessageService messageService, CoreContext context)
        {
            _messageService = messageService;
            _context = context;
        }

        [HttpPost]
        [Route("receive")]
        public IActionResult ReceiveMessage(SmsRequest request)
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message(request.Body);

            return TwiML(messagingResponse);
        }

        [HttpPost, Route("send")]
        public async Task<IActionResult> SendMessage(SendMessageRequest sendMessageRequest)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == sendMessageRequest.CustomerId);
            var id = await _messageService.Send(sendMessageRequest.Content, "+14155238886", customer.Phone);

            return Ok(id);
        }
    }
}