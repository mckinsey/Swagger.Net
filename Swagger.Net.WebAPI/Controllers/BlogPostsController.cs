using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swagger.Net.WebAPI.Models;

namespace Swagger.Net.WebApi.Controllers
{
    /// <summary>
    /// BlogPosts Controller
    /// </summary>
    public class BlogPostsController : ApiController
    {
        // GET api/blogposts
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Blog> Get()
        {
            return LoadBlogs();
        }

        // GET api/blogposts/5
        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Blog Get(int id)
        {
            return LoadBlogs().FirstOrDefault(b => b.Id == id); ;
        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<Blog> GetBlogsByType(Blogtype type)
        {
            return LoadBlogs().Where(b => b.Type == type);
        }

        // POST api/blogposts
        public void Post([FromBody]string value)
        {
        }

        // PUT api/blogposts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/blogposts/5
        public void Delete(int id)
        {
        }


        /// <summary>
        /// Loads the blogs.
        /// </summary>
        /// <returns></returns>
        List<Blog> LoadBlogs()
        {
            return new List<Blog> {
                new Blog{Auther="swagger",Content="content 1",Id=1,Title="testSocial",Type=Blogtype.Social},
                new Blog{Auther="swagger",Content="content 2",Id=2,Title="testHealth",Type=Blogtype.Health},
                new Blog{Auther="swagger",Content="content 3",Id=2,Title="testTechnical",Type=Blogtype.Technical},
                new Blog{Auther="swagger",Content="content 4",Id=2,Title="testSocial",Type=Blogtype.Social},
                new Blog{Auther="swagger",Content="content 5",Id=2,Title="testHealth",Type=Blogtype.Health},
                new Blog{Auther="swagger",Content="content 6",Id=2,Title="testTechnical",Type=Blogtype.Technical},
            };
        }
    }
}
