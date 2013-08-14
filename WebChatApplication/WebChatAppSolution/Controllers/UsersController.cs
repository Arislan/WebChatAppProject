using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.Repository;
using WebChat.Data;
using WebChat.Models;

namespace WebChatAppSolution.Controllers
{
    public class UsersController : ApiController
    {
        private IRepositoty<User> userRepository;

        public UsersController()
        {
        }

        public UsersController(IRepositoty<User> userRepository)
        {
            this.userRepository = userRepository ;
        }

        // GET api/User
       
    }
}
