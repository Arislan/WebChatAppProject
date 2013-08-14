using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace WebChatAppSolution.Models
{
    [DataContract]
    public class UserByChannels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NickName { get; set; }
    }
}