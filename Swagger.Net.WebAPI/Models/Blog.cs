using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swagger.Net.WebAPI.Models
{
    /// <summary>
    /// Blog Model
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the auther.
        /// </summary>
        /// <value>
        /// The auther.
        /// </value>
        public string Auther { get; set; }
        
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Blogtype Type { get; set; }
    }

    /// <summary>
    /// Blog Type Enum
    /// </summary>
    public enum Blogtype
    {
        /// <summary>
        /// The social
        /// </summary>
        Social,
        /// <summary>
        /// The technical
        /// </summary>
        Technical,
        /// <summary>
        /// The health
        /// </summary>
        Health,
        /// <summary>
        /// The others
        /// </summary>
        Others
    }
}