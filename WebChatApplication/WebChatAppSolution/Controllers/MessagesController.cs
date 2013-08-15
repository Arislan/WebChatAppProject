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
        private IRepositoty<Channel> channelRepository;

        private PubnubAPI pubnub = new PubnubAPI(
          "pub-c-01dc0145-d006-4cc3-b34f-5b4f101cdaa5",               // PUBLISH_KEY
          "sub-c-3e6b5fb6-04d9-11e3-8dc9-02ee2ddab7fe",               // SUBSCRIBE_KEY
          "sec-c-NWI4MThmOTYtNWE1My00MmVlLWI1ZTktNzZmODgyOTIyZWY0",   // SECRET_KEY
          true                                                        // SSL_ON?
      );

        public MessagesController()
        {
        }

        public MessagesController(IRepositoty<Message> messageRepository, IRepositoty<User> userRepository, 
            IRepositoty<Channel> channelRepo)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.channelRepository = channelRepo;
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
                    State = x.State,

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
                    State = x.State,

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
            Channel channel = this.channelRepository.Find( x => x.Name == value.Channel).FirstOrDefault();
            if( channel == null)
            {
                this.channelRepository.Add(new Channel { Name = value.Channel });
                channel = this.channelRepository.Find(x => x.Name == value.Channel).FirstOrDefault();
            }

            Message message = CreateMessage(value);
            message.PublishDate = DateTime.Now;
            pubnub.Publish(value.Channel, String.Format("[{0}]{1} - {2}", message.PublishDate.ToString("hh:mm:ss"), value.Sender.NickName, value.Content));
           
            message.Channel = channel;
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
            message.State = value.State ?? message.State;

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
                State = value.State,
            };

            User sender = new User() ;
            if (value.Sender != null)
            {
                sender = this.userRepository.Find(x => x.NickName == value.Sender.NickName).FirstOrDefault();
            }

            if (sender != null)
            {
                newMessage.Sender = sender;
            }
            else
            {
                throw new ArgumentNullException("User must be register");
            }

            User resiver = new User();

            if (value.Retriever != null)
            {
                resiver = this.userRepository.Find(x => x.Id == value.Retriever.Id).FirstOrDefault();
            }

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
                 State = message.State,

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
