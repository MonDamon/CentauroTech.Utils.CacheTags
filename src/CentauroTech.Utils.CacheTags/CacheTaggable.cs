

using System;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;

namespace CentauroTech.Utils.CacheTags
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheTaggable : System.Web.Http.Filters.ActionFilterAttribute
    {
        readonly bool _enableCacheTag = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCacheTag"] ?? "false");
        private string _parametersToCache = string.Empty;

        public CacheTaggable(string parametersToCache)
        {
            _parametersToCache = parametersToCache;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Request.Method.Equals(HttpMethod.Get) && _enableCacheTag)
            {
                var taggedController = (CacheTaggedApiController)actionContext.ControllerContext.Controller;
                if(!string.IsNullOrWhiteSpace(_parametersToCache))
                ((List<string>)taggedController.QueryStringToCheck).AddRange(_parametersToCache.Split(',').Select(x=>x));
                taggedController.AddCacheTag = true;
            }
        }
    }
}
