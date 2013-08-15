using System;
using System.Linq;

namespace WebChat.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string FileUrl { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string State { get; set; }

        public int SenderId { get; set; }
        public virtual User Retriever { get; set; }

        public int RetrieverId { get; set; }
        public virtual User Sender { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
