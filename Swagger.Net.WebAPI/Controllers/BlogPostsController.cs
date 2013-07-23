using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogPostsController : ApiController
    {
        // GET api/blogposts
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/blogposts/5
        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value";
        }

        // POST api/blogposts
        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/blogposts/5
        /// <summary>
        /// Puts the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/blogposts/5
        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(int id)
        {
        }
    }
}
