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
    public class ChannelController : ApiController
    {
        private IRepositoty<Channel> channelRepository;

        public ChannelController()
        {
        }

        public ChannelController(IRepositoty<Channel> channelRepository)
        {
            this.channelRepository = channelRepository;
        }

        // GET api/channel
        public IEnumerable<ChannelModel> Get()
        {
            var AllChannels = channelRepository.All();
            IEnumerable<ChannelModel> channels = AllChannels.Select(
                x => new ChannelModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Users = x.Users.Select(y => new UserByChannels()
                    {
                        Id = y.Id,
                        NickName = y.NickName,
                    })

                });



            return channels;
        }

        // GET api/channel/5
        public ChannelModel Get(int id)
        {
            var AllChannels = channelRepository.All();
            ChannelModel channel = channelRepository
                .Find(x => x.Id == id)
                .Select(
                x => new ChannelModel()
                {
                    Id = x.Id,
                    Name = x.Name,

                    Users = x.Users.Select(y => new UserByChannels()
                    {
                        Id = y.Id,
                        NickName = y.NickName,
                    })
                }).FirstOrDefault();
            return channel;
        }

        // POST api/channel
        public void Post([FromBody]ChannelModel value)
        {
            Channel channel = new Channel()
            {
                Id = value.Id,
                Name = value.Name,
            };
            foreach (var user in value.Users)
            {
                User newUser = new User()
                {
                    Id = user.Id,
                    NickName = user.NickName
                };
                channel.Users.Add(newUser);
            }

            channelRepository.Add(channel);
        }

        // PUT api/channel/5
        public void Put(int id, [FromBody]ChannelModel value)
        {
            var channel = channelRepository.Find(x => x.Id == id).FirstOrDefault();

         

            channelRepository.Update(channel);
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
