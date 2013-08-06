using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Filters;

namespace Swagger.Net
{
    /// <summary>
    /// Determines if any request hit the Swagger route. Moves on if not, otherwise responds with JSON Swagger spec doc
    /// </summary>
    public class SwaggerActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Executes each request to give either a JSON Swagger spec doc or passes through the request
        /// </summary>
        /// <param name="actionContext">Context of the action</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var docRequest = actionContext.ControllerContext.RouteData.Values.ContainsKey(SwaggerGen.SWAGGER);

            if (!docRequest)
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            HttpResponseMessage response = new HttpResponseMessage();

            response.Content = new ObjectContent<ResourceListing>(
                getDocs(actionContext),
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);

            actionContext.Response = response;
        }

        private ResourceListing getDocs(HttpActionContext actionContext)
        {
            var docProvider = (XmlCommentDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

            ResourceListing r = SwaggerGen.CreateResourceListing(actionContext);

            var existing = new Dictionary<HttpMethod, List<ResourceApi>>();

            foreach (var api in GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions
                .OrderBy(api => api.HttpMethod.Method).ThenBy(api => api.RelativePath.Length))
            {
                string apiControllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
                if (api.Route.Defaults.ContainsKey(SwaggerGen.SWAGGER) ||
                    apiControllerName.ToUpper().Equals(SwaggerGen.SWAGGER.ToUpper()))
                    continue;

                // Make sure we only report the current controller docs
                if (!apiControllerName.Equals(actionContext.ControllerContext.ControllerDescriptor.ControllerName))
                    continue;

                if (!existing.ContainsKey(api.HttpMethod))
                    existing[api.HttpMethod] = new List<ResourceApi>();

                ResourceApi rApi = SwaggerGen.CreateResourceApi(api);
                if (IsDuplicate(api, rApi, existing[api.HttpMethod]))
                    continue;
                existing[api.HttpMethod].Add(rApi);
                r.apis.Add(rApi);

                ResourceApiOperation rApiOperation = SwaggerGen.CreateResourceApiOperation(api, docProvider);
                rApi.operations.Add(rApiOperation);

                foreach (var param in api.ParameterDescriptions)
                {
                    ResourceApiOperationParameter parameter = SwaggerGen.CreateResourceApiOperationParameter(api, param, docProvider);
                    rApiOperation.parameters.Add(parameter);
                }
            }

            return r;
        }

        private bool IsDuplicate(ApiDescription api, ResourceApi rapi, List<ResourceApi> existing)
        {
            if (!api.HttpMethod.Method.Equals(api.ActionDescriptor.ActionName, StringComparison.CurrentCultureIgnoreCase))
                return false;

            var controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = api.ActionDescriptor.ActionName;

            var uglySyntax = string.Format("/api/{0}/{1}", controllerName, actionName);
            var prettySyntax = string.Format("/api/{0}", controllerName);

            if (!rapi.path.StartsWith(uglySyntax, StringComparison.CurrentCultureIgnoreCase))
                return false;

            return existing.Any(e => e.path.StartsWith(prettySyntax, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}