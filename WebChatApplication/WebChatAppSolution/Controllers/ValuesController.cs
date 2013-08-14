using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.Data;
using WebChat.Models;

namespace WebChatAppSolution.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5", "value6" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            var db = new WebChatEntity();

            db.Users.Add(new User { NickName = "GGggggg" });  
            db.SaveChanges();
            return ConfigurationManager.AppSettings["ChatDbConnectionString"];
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}