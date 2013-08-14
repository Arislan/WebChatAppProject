using System;
using System.Linq;

namespace WebChat.Models
{
    public class User
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public string HashedPass { get; set; }

        public string PictureUrl { get; set; }

        public string SessionKey { get; set; }
    }
}
