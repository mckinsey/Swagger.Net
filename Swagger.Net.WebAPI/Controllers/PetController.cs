using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Swagger_Test
{
    /// <summary>
    ///     A WebAPI example that uses HttpResponseMessage for return values along with custom routes
    ///     to better cover Swagger's interop with WebAPI
    /// </summary>
    /// <remarks>
    ///   See http://stackoverflow.com/questions/10660721/what-is-the-difference-between-httpresponsemessage-and-httpresponseexception
    ///   for a discussion on the merits of using HttpResponseMessage over other return types.
    /// </remarks>
    public class PetController : ApiController
    {
        /// <summary>
        ///     Retuns a paged list of pet names
        ///     GET api/Pet
        /// </summary>
        /// <remarks>
        ///     Remarks for Get(int page = 1, int? size=10)
        /// </remarks>
        /// <returns cref="Pet" type="Pet[]"></returns>
        /// <response code="400">Bad request; parameters {page} and/or {size} were invalid</response>
        /// <response code="401">Authentication credentials required for this API</response>
        /// <response code="403">Account not authorized to access this entity</response>
        public HttpResponseMessage Get(int page = 1, int? size=10)
        {
            return  Request.CreateResponse(HttpStatusCode.OK,  new Pet[] { new Pet() { Id=1, Name="Pet #1"} , new Pet() { Id = 2, Name = "Pet #2" } });
        }

        /// <summary>
        ///     Retuns a Pet
        ///     GET api/Pet/{id}
        /// </summary>
        /// <returns cref="Pet" type="Pet"></returns>
        /// <response code="400">Bad request; parameter {id} was invalid</response>
        /// <response code="401">Authentication credentials required for this API</response>
        /// <response code="403">Account not authorized to access this entity</response>
        /// <response code="404">The Pet entity was not found</response>
        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, String.Concat("The value of id is ", id));
        }

        // GET api/Pet/5
        [HttpGet]
        [ActionName("Export")]
        public HttpResponseMessage GetExport(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, String.Concat("This is an exported file for id #", id));
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Pet value)
        {
            // addd Location: header with the Id value
            return Request.CreateResponse(HttpStatusCode.Created); // 201
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]Pet value)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);

        }

        // PUT api/<controller>/5
        [HttpPut]
        [ActionName("PutExport")]
        public HttpResponseMessage Put(int id, int? userId = null)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class Pet
    {
        public int Id;
        public string Name;
    }
}