

using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System;
using log4net;
using System.Web;
using System.Web.Mvc;

namespace CentauroTech.Utils.CacheTags
{
    public class CacheTaggedController : AsyncController
    {

        private const string ENABLECACHETAG = "EnableCacheTag";
        private const string EDGECACHETAG = "EdgeCacheTag";
        private const string EDGECACHETAGVALUE = "Edge-Cache-Tag";
        private readonly bool _enableCacheTag = Convert.ToBoolean(ConfigurationManager.AppSettings[ENABLECACHETAG] ?? "false");
        private readonly string _edgeCacheTag = ConfigurationManager.AppSettings[EDGECACHETAG] ?? EDGECACHETAGVALUE;

        protected Func<IEnumerable<string>, IEnumerable<string>> CacheTagFormatter = tags =>
        {
            return tags;
        };
        protected virtual void AddCacheTagsHeader(HttpContextBase httpContext, IEnumerable<string> tags)
        {                
                if (_enableCacheTag && tags.Any() && !httpContext.Response.Headers.AllKeys.Any(x => x.Equals(_edgeCacheTag)))
                {
                    httpContext.Response.Headers.Add(_edgeCacheTag, string.Join(",", CacheTagFormatter(tags)));
                }            
        }

    }

}

