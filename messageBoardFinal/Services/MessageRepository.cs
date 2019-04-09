using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MessageBoardBackend.Core;
using MessageBoardBackend.Models;

namespace MessageBoardBackend.Services
{

    public class MessageRepository
    {
        private ApiContext _context;
        public MessageRepository()
        {
            _context = new ApiContext();
        }
        public IEnumerable<Message> GetAllMessages()
        {
           
            var messages = _context.Messages.ToList();
            return messages;
        }

        public IEnumerable<Message> GetMessage(string name)
        {
            var messages = _context.Messages
                .Where(x=>x.Owner== name).ToList();
            return messages;
        }
        public IEnumerable<Message> GetMessage(Guid id)
        {
            var messages = _context.Messages
                .Where(x => x.Id == id).ToList();
            return messages;
        }

        public void Insert( Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}