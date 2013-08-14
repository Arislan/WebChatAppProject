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
        private IRepositoty<User> userRepository;

        public MessagesController()
        {
        }

        public MessagesController(IRepositoty<Message> messageRepository, IRepositoty<User> userRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
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

        // GET api/messages/1
        public MessagesDto Get(int id)
        {
            var categoryDto = this.messageRepository
                .Find(x => x.Id == id)
                .Select(
                x => new MessagesDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    FileUrl = x.FileUrl,
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
                    },
                })
                .FirstOrDefault();

            return categoryDto;
        }

        // POST api/categories
        //
        public HttpResponseMessage Post([FromBody]MessagesDto value)
        {
            Message message = CreateMessage(value);

            messageRepository.Add(message);

            var response = this.BuildHttpResponse(message, HttpStatusCode.Created);
            return response;
        }

        // PUT api/places/5
        public HttpResponseMessage Put(int id, [FromBody]MessagesDto value)
        {
            var message = this.messageRepository.Find(x => x.Id == id).FirstOrDefault();

            message.FileUrl = value.FileUrl ?? message.FileUrl;
            message.Content = value.Content ?? message.Content;

            if (value.PublishDate != null)
            {
                message.PublishDate = value.PublishDate;
            }

            if (value.Sender != null)
            {
                message.Sender = new User()
                {
                    Id = value.Sender.Id,
                    NickName = value.Sender.NickName,
                };
            }

            if (value.Retriever != null)
            {
                message.Retriever = new User()
                {
                    Id = message.RetrieverId,
                    NickName = message.Retriever.NickName,
                };
            }

            this.messageRepository.Update(message);
            var response = this.BuildHttpResponse(message, HttpStatusCode.OK);
            return response;
        }

        //// DELETE api/places/5
        //public void Delete(int id)
        //{
        //    this.messageRepository.Delete(id);
        //}

        private Message CreateMessage(MessagesDto value)
        {
            var newMessage = new Message()
            {
                Id = value.Id,
                Content = value.Content,
                FileUrl = value.FileUrl,
                PublishDate = value.PublishDate,
            };

            User sender = this.userRepository.Find(x => x.Id == value.Sender.Id).FirstOrDefault();

            if (sender != null)
            {
                newMessage.Sender = sender;
            }
            else
            {
                throw new ArgumentNullException("User must be register");
            }

            User resiver = this.userRepository.Find(x => x.Id == value.Retriever.Id).FirstOrDefault();

            if (resiver != null)
            {
                newMessage.Retriever = resiver;
            }
            else
            {
                throw new ArgumentNullException("User must be register");
            }

            return newMessage;
        }

        private HttpResponseMessage BuildHttpResponse(Message message, HttpStatusCode code)
        {
            var response =
             Request.CreateResponse(code, new MessagesDto()
             {
                 Id = message.Id,
                 FileUrl = message.FileUrl,
                 Content = message.Content,
                 PublishDate = message.PublishDate,

                 Retriever = new UsersByMessages()
                 {
                     Id = message.Retriever.Id,
                     NickName = message.Retriever.NickName,
                 },

                 Sender = new UsersByMessages()
                 {
                     Id = message.Sender.Id,
                     NickName = message.Sender.NickName,
                 },
             });

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = message.Id }));

            return response;
        }
    }
}
