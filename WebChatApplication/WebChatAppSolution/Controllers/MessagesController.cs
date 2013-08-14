using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.Repository;
using WebChat.Data;
using WebChat.Models;
using WebChatAppSolution.Models;

namespace WebChatAppSolution.Controllers
{
    public class MessagesController : ApiController
    {
        private IRepositoty<Message> messageRepository;

        public MessagesController()
        {
        }

        public MessagesController(IRepositoty<Message> messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        // GET api/messages
        public IEnumerable<MessagesDto> Get()
        {
            var AllCategories = this.messageRepository.All();

            IEnumerable<MessagesDto> Dtos = AllCategories.Select(
                x => new MessagesDto()
                {
                    Id = x.Id,
                    FileUrl = x.FileUrl,
                    Content = x.Content,
                    PublishDate = x.PublishDate,

                    Retriever = new UsersByMessages()
                    {
                        Id = x.Retriever.Id,
                        NickName = x.Retriever.NickName,
                    },

                    Sender = new UsersByMessages()
                    {
                        Id = x.Sender.Id,
                        NickName = x.Sender.NickName,
                    }
                });

            return Dtos;
        }
       
    }
}
