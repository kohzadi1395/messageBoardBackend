using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageBoardBackend.Core;
using MessageBoardBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoardBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly MessageRepository messageRepository;

        public MessagesController()
        {
            messageRepository = new MessageRepository();
        }

        public IEnumerable<Models.Message> Get()
       {
            return messageRepository.GetAllMessages();
        }

        [HttpGet("{name}")]
        public IEnumerable<Models.Message> Get(string name)
        {
            return messageRepository.GetMessage(name);
        }

        [HttpPost]
        public void Post([FromBody] Models.Message message)
        {
             messageRepository.Insert(message);
        }
    }
}
