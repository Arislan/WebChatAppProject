using System;
using System.Linq;
using WebChat.Models;
using System.Runtime.Serialization;

namespace WebChatAppSolution.Models
{
    [DataContract]
    public class ChannelModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

        public static ChannelModel CreateFromChannelEntity(Channel channelEntity)
        {
            ChannelModel model = new ChannelModel()
            {
                Id = channelEntity.Id,
                Name = channelEntity.Name
            };

            return model;
        }
    }
}