using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebChat.Models;

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
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "pictureUrl")]
        public string PictureUrl { get; set; }

        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        public static UserLoggedModel CreateFromUserEntity(User userEntity)
        {
            UserLoggedModel model = new UserLoggedModel()
            {
                Id = userEntity.Id,
                Nickname = userEntity.NickName,
                SessionKey = userEntity.SessionKey,
                PictureUrl = userEntity.PictureUrl
            };

            return model;
        }
    }

    [DataContract]
    public class UserRegisteredModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        public static UserRegisteredModel CreateFromUserEntity(User userEntity)
        {
            UserRegisteredModel model = new UserRegisteredModel()
            {
                Id = userEntity.Id,
                Nickname = userEntity.NickName
            };

            return model;
        }
    }
}