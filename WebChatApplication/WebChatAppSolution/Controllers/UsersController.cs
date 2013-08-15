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
            this.userRepository = userRepository;
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

            var response = this.LoginUser(createdUser, HttpStatusCode.Created);
            return response;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser(UserModel user)
        {
            string userNicknameToLower = user.NickName.ToLower();

            var existingUser = this.userRepository.Find
                (u => u.NickName == userNicknameToLower && u.HashedPass == user.HashedPass).FirstOrDefault();

            if (existingUser == null)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Incorrect nickname or password");
                return errResponse;
            }

            var response = this.LoginUser(existingUser, HttpStatusCode.OK);
            return response;
        }

        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(string sessionKey)
        {
            var user = this.userRepository.Find(u => u.SessionKey == sessionKey).FirstOrDefault();

            if (user == null)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "There is no session with the given session key");
                return errResponse;
            }

            user.SessionKey = null;
            this.userRepository.Update(user);

            var response = this.Request.CreateResponse(HttpStatusCode.OK,
                "User logged out successfully");
            return response;
        }

        [HttpGet]
        [ActionName("logged")]
        public IEnumerable<UserLoggedModel> GetLoggedUsers()
        {
            var loggedUsers = this.userRepository.Find(u => u.SessionKey != null);

            List<UserLoggedModel> models = new List<UserLoggedModel>();
            foreach (var user in loggedUsers)
            {
                models.Add(UserLoggedModel.CreateFromUserEntity(user));
            }

            return models;
        }

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<UserRegisteredModel> GetRegisteredUsers()
        {
            var users = this.userRepository.All();

            List<UserRegisteredModel> models = new List<UserRegisteredModel>();
            foreach (var user in users)
            {
                models.Add(UserRegisteredModel.CreateFromUserEntity(user));
            }

            return models;
        }

        private HttpResponseMessage LoginUser(User user, HttpStatusCode statusCode)
        {
            string sessionKey = this.GenerateSessionKey(user.Id);

            user.SessionKey = sessionKey;

            this.userRepository.Update(user);

            UserLoggedModel loggedUser = UserLoggedModel.CreateFromUserEntity(user);

            var response = this.Request.CreateResponse(statusCode, loggedUser);
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
