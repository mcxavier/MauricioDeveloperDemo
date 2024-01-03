using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]"), ApiController]
    public class MessagesController : ControllerBase
    {

        [HttpPost]
        public void Post(string message)
        { 
        }
    }
}