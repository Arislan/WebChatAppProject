using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.Repository;
using WebChat.Data;
using WebChat.Models;
using WebChatAppSolution.Models;
using System.Text;

namespace WebChatAppSolution.Controllers
{
    public class UsersController : ApiController
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        private IRepositoty<User> userRepository;

        public UsersController()
        {
        }

        public UsersController(IRepositoty<User> userRepository)
        {
            this.userRepository = userRepository ;
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser(UserModel user)
        {
            var existingUser = this.userRepository.Find(u => u.NickName.ToLower() == user.NickName.ToLower()).FirstOrDefault();
            if (existingUser != null)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "A user with that nickname already exists");
                return errResponse;
            }

            string nicknameToLower = user.NickName.ToLower();

            User userEntity = new User()
            {
                NickName = nicknameToLower,
                HashedPass = user.HashedPass
            };

            var createdUser = this.userRepository.Add(userEntity);
            string sessionKey = this.GenerateSessionKey(createdUser.Id);

            createdUser.SessionKey = sessionKey;

            this.userRepository.Update(createdUser);

            UserLoggedModel loggedUser = new UserLoggedModel()
            {
                Nickname = createdUser.NickName,
                SessionKey = sessionKey
            };

            var response = this.Request.CreateResponse(HttpStatusCode.Created, loggedUser);
            return response;
        }

        private string GenerateSessionKey(int userId)
        {
            Random rand = new Random();

            StringBuilder keyChars = new StringBuilder(50);
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }
    }
}
