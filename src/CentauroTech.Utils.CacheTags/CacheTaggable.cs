

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
        private readonly bool _enableCacheTag = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCacheTag"] ?? "false");
      
        public CacheTaggable(string parametersToCache)
        {
            _parametersToCache = parametersToCache;
        }

        private string _parametersToCache { get; set; }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Request.Method.Equals(HttpMethod.Get) && _enableCacheTag)
            {
                if (!string.IsNullOrWhiteSpace(_parametersToCache))
                {
                    ((List<string>)((CacheTaggedApiController)actionContext.ControllerContext.Controller).QueryStringToCheck).AddRange(_parametersToCache.Split(',').Select(x => x));
                }
            }
            else
            {
                ((CacheTaggedApiController)actionContext.ControllerContext.Controller).AddCacheTag = false;
            }
        }
    }
}
