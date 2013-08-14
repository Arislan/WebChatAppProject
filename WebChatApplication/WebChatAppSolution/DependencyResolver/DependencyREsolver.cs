using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using WebChatAppSolution.Controllers;
using WebChat.Data;
using WebChat.Models;
using WebChat.Repository;

namespace WebChatAppSolution.DependencyResolver
{
    public class DbDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            //here we can write what to do for all controllers we can use something differen than EntityFramework
            if (serviceType == typeof(UsersController))
            {
                var userRepository = new EFRepository<User>(new WebChatEntity());
                return new UsersController(userRepository);
            }
            //else if (serviceType == typeof(CommentsController))
            //{
            //    var dbContext = new PlacesContext();
            //    var placeRepository = new EFRepository<Place>(dbContext);
            //    var commentsRepository = new EFRepository<Comment>(dbContext);
            //    return new CommentsController(commentsRepository, placeRepository);
            //}
            //else if (serviceType == typeof(PlacesController))
            //{
            //    var dbContext = new PlacesContext();
            //    var placeRepository = new EFRepository<Place>(dbContext);
            //    var commentsRepository = new EFRepository<Comment>(dbContext);
            //    var categoryRepository = new EFRepository<Category>(dbContext);
            //    return new PlacesController(placeRepository, commentsRepository, categoryRepository);
            //}
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}