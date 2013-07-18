using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swagger.Net
{
    public static class SwaggerGen
    {
        public const string SWAGGER = "swagger";
        public const string SWAGGER_VERSION = "2.0";
        public const string FROMURI = "FromUri";
        public const string FROMBODY = "FromBody";
        public const string QUERY = "query";
        public const string PATH = "path";
        public const string BODY = "body";

        /// <summary>
        /// Create a resource listing
        /// </summary>
        /// <param name="actionContext">Current action context</param>
        /// <param name="includeResourcePath">Should the resource path property be included in the response</param>
        /// <returns>A resource Listing</returns>
        public static ResourceListing CreateResourceListing(HttpActionContext actionContext, bool includeResourcePath = true)
        {
            return CreateResourceListing(actionContext.ControllerContext, includeResourcePath);
        }

        /// <summary>
        /// Create a resource listing
        /// </summary>
        /// <param name="actionContext">Current controller context</param>
        /// <param name="includeResourcePath">Should the resource path property be included in the response</param>
        /// <returns>A resrouce listing</returns>
        public static ResourceListing CreateResourceListing(HttpControllerContext controllerContext, bool includeResourcePath = false)
        {
            Uri uri = controllerContext.Request.RequestUri;

            ResourceListing rl = new ResourceListing()
            {
                apiVersion = Assembly.GetCallingAssembly().GetType().Assembly.GetName().Version.ToString(),
                swaggerVersion = SWAGGER_VERSION,
                basePath = uri.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/'),
                apis = new List<ResourceApi>(),
                models=new Dictionary<string,ResourceModel>()
            };

            if (includeResourcePath) rl.resourcePath = controllerContext.ControllerDescriptor.ControllerName;

            return rl;
        }

        /// <summary>
        /// Create an api element 
        /// </summary>
        /// <param name="api">Description of the api via the ApiExplorer</param>
        /// <returns>A resource api</returns>
        public static ResourceApi CreateResourceApi(ApiDescription api)
        {
            ResourceApi rApi = new ResourceApi()
            {
                path = "/" + api.RelativePath,
                description = api.Documentation,
                operations = new List<ResourceApiOperation>()
            };

            return rApi;
        }

        /// <summary>
        /// Creates the resource model.
        /// </summary>
        /// <param name="api">Description of the api via the ApiExplorer.</param>
        /// <returns>A resource model</returns>
        public static List<ResourceModel> CreateResourceModel(Type returnType)
        {

            if(returnType ==null)
                return null;
            if (returnType.IsGenericType)
                returnType = returnType.GetGenericArguments()[0];
            if (IsSimpleType(returnType)) return null;

            List<ResourceModel> modelArray = new List<ResourceModel>();
            ResourceModel model = new ResourceModel();
            model.id = returnType.Name;
            model.name = returnType.Name;
            model.properties = new Dictionary<string,ResourceModelProperty>();
            var properties = returnType.GetProperties();

            foreach (var property in properties)
            {
                ResourceModelProperty prop = new ResourceModelProperty();
                prop.required = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Any();
                prop.type = GetTypeAsString(property.PropertyType);

                model.properties.Add(property.Name, prop);

                if (!IsSimpleType(property.PropertyType))
                {
                    modelArray.AddRange(CreateResourceModel(property.PropertyType));
                }
            }
            modelArray.Add(model);
            return modelArray;
        }
        private static bool IsSimpleType(Type type)
        {
            if (type.IsPrimitive || type.Assembly.FullName.Contains("mscorlib"))
                return true;
            return false;
        }

        /// <summary>
        /// Creates an api operation
        /// </summary>
        /// <param name="api">Description of the api via the ApiExplorer</param>
        /// <param name="docProvider">Access to the XML docs written in code</param>
        /// <returns>An api operation</returns>
        public static ResourceApiOperation CreateResourceApiOperation(ApiDescription api, XmlCommentDocumentationProvider docProvider)
        {
            ResourceApiOperation rApiOperation = new ResourceApiOperation()
            {
                httpMethod = api.HttpMethod.ToString(),
                nickname = docProvider.GetNickname(api.ActionDescriptor),
                responseClass = docProvider.GetResponseClass(api.ActionDescriptor),
                summary = api.Documentation,
                notes = docProvider.GetNotes(api.ActionDescriptor),
                parameters = new List<ResourceApiOperationParameter>()
            };

            return rApiOperation;
        }

        /// <summary>
        /// Creates an operation parameter
        /// </summary>
        /// <param name="api">Description of the api via the ApiExplorer</param>
        /// <param name="param">Description of a parameter on an operation via the ApiExplorer</param>
        /// <param name="docProvider">Access to the XML docs written in code</param>
        /// <returns>An operation parameter</returns>
        public static ResourceApiOperationParameter CreateResourceApiOperationParameter(ApiDescription api, ApiParameterDescription param, XmlCommentDocumentationProvider docProvider)
        {
            string paramType = (param.Source.ToString().Equals(FROMURI)) ? QUERY : BODY;
            ResourceApiOperationParameter parameter = new ResourceApiOperationParameter()
            {
                paramType = (paramType == "query" && api.RelativePath.IndexOf("{" + param.Name + "}") > -1) ? PATH : paramType,
                name = param.Name,
                description = param.Documentation,
                dataType = param.ParameterDescriptor.ParameterType.Name,
                required = docProvider.GetRequired(param.ParameterDescriptor)
            };

            return parameter;
        }

        /// <summary>
        /// Gets the type as string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetTypeAsString(Type type)
        {
            if (type.IsGenericType)
            {
                StringBuilder sb = new StringBuilder(type.Name);
                sb.Append("&lt;");
                Type[] types = type.GetGenericArguments();
                for (int i = 0; i < types.Length; i++)
                {
                    sb.Append(GetTypeAsString(types[i]));
                    if (i != (types.Length - 1)) sb.Append(", ");
                }
                sb.Append("&gt;");
                return sb.Replace("`1", "").ToString();
            }
            else
                return type.Name;
        }
        
    }

    public class ResourceListing
    {
        public string apiVersion { get; set; }
        public string swaggerVersion { get; set; }
        public string basePath { get; set; }
        public string resourcePath { get; set; }
        public List<ResourceApi> apis { get; set; }
        public Dictionary<string,ResourceModel> models { get; set; }
    }

    public class ResourceApi
    {
        public string path { get; set; }
        public string description { get; set; }
        public List<ResourceApiOperation> operations { get; set; }
    }

    public class ResourceApiOperation
    {
        public string httpMethod { get; set; }
        public string nickname { get; set; }
        public string responseClass { get; set; }
        public string summary { get; set; }
        public string notes { get; set; }
        public List<ResourceApiOperationParameter> parameters { get; set; }
    }

    public class ResourceApiOperationParameter
    {
        public string paramType { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string dataType { get; set; }
        public bool required { get; set; }
        public bool allowMultiple { get; set; }
        public OperationParameterAllowableValues allowableValues { get; set; }
    }

    public class OperationParameterAllowableValues
    {
        public int max { get; set; }
        public int min { get; set; }
        public string valueType { get; set; }
    }

    public class ResourceModel
    {
        public string id { get; set; }
        public string name { get; set; }

        public Dictionary<string,ResourceModelProperty> properties { get; set; }


    }

    public class ResourceModelProperty
    {
        public string type { get; set; }
        public bool required { get; set; }
    }
}