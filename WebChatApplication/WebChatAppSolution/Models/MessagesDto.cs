using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Models;
using System.Runtime.Serialization;

namespace WebChatAppSolution.Models
{
    [DataContract]
    public class MessagesDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime PublishDate { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public virtual UsersByMessages Retriever { get; set; }

        [DataMember]
        public virtual UsersByMessages Sender { get; set; }
    }
}