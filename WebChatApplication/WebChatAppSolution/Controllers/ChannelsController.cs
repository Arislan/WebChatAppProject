using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.Data;
using WebChat.Models;
using WebChat.Repository;
using WebChatAppSolution.Models;

namespace WebChatAppSolution.Controllers
{
    public class ChannelsController : ApiController
    {
        private IRepositoty<Channel> channelRepository;

        public ChannelsController()
        {
        }

        public ChannelsController(IRepositoty<Channel> channelRepository)
        {
            this.channelRepository = channelRepository;
        }

        // GET api/channels
        public IEnumerable<ChannelModel> Get()
        {
            var channels = this.channelRepository.All();

            List<ChannelModel> models = new List<ChannelModel>();
            foreach (var channel in channels)
            {
                models.Add(ChannelModel.CreateFromChannelEntity(channel));
            }

            return models;
        }

        // GET api/channels/5
        public ChannelModel Get(int id)
        {
            var channelEntity = this.channelRepository.Get(id);
            ChannelModel model = ChannelModel.CreateFromChannelEntity(channelEntity);

            return model;
        }

        // POST api/channels
        public HttpResponseMessage Post(ChannelModel channelModel)
        {
            if (channelModel.Name == null)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "The channels does not have a name");
                return errResponse;
            }

            Channel entity = new Channel()
            {
                Name = channelModel.Name
            };

            this.channelRepository.Add(entity);

            var response = this.Request.CreateResponse(HttpStatusCode.Created, entity);
            return response;
        }

        // DELETE api/channel/5
        public void Delete(int id)
        {
            var channel = channelRepository
                 .Find(x => x.Id == id).FirstOrDefault();

            channelRepository.Delete(channel);
        }
    }
}
