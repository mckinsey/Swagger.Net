using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net
{
    /// <summary>
    /// Attribute can be used in controllers and actions to provide more information to SwaggerAPI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ApiAwareAttribute : Attribute
    {
        public ApiAwareAttribute()
        {

        }
        public ApiAwareAttribute(Type modelType)
        {
            this.ModelType = modelType;
        }
        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; set; }

        /// <summary>
        /// Gets or sets the exception types.
        /// </summary>
        /// <value>
        /// The exception types.
        /// </value>
        public Type[] ExceptionTypes { get; set; }
    }
}
