using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Swagger.Net.WebApi.Models;

namespace Swagger.Net.WebApi.Controllers
{
    public class PetsController : ApiController
    {
        //
        // GET: /Pets/

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>List of Pets</returns>
        public IEnumerable<Pet> Get()
        {
            IEnumerable<Pet> pets = GetPets();
            return pets;
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Pet</returns>
        public Pet Get(int id)
        {
            return GetPets().FirstOrDefault(p => p.Id.Equals(id));
        }


        private IEnumerable<Pet> GetPets()
        {
            return new List<Pet> {
            new Pet{Id=1,Name="Cat"},
            new Pet{Id=2,Name="Dog"}
            
            };
        }

    }
}
