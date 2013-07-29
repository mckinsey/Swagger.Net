using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swagger.Net.WebAPI.Models
{

    /// <summary>
    /// Pet model
    /// </summary>
    public class Pet
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }

    /// <summary>
    /// PetType Enum
    /// </summary>
    public enum PetType
    {
        /// <summary>
        /// The bird
        /// </summary>
        Bird,
        /// <summary>
        /// The animal
        /// </summary>
        Animal,
        /// <summary>
        /// The fish
        /// </summary>
        Fish
    }
}