using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebChatAppSolution.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember(Name = "nickname")]
        public string NickName { get; set; }

        [DataMember(Name = "hashedPass")]
        public string HashedPass { get; set; }
    }

    [DataContract]
    public class UserLoggedModel
    {
        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
    }
}