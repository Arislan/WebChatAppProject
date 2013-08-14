using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Models;
using System.Runtime.Serialization;

namespace WebChatAppSolution.Models
{
    [DataContract]
    public class ChannelModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<UserByChannels> Users { get; set; }

    }
}