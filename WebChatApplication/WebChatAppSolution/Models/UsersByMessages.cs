using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebChatAppSolution.Models
{
    [DataContract]
    public class UsersByMessages
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string NickName { get; set; }
    }
}