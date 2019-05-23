
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System;
using log4net;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;

namespace CentauroTech.Utils.CacheTags
{ 
    public class CacheTaggedController: AsyncController
    {    
        readonly bool _enableCacheTag = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCacheTag"] ?? "false");
        readonly string _edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTag"] ?? "Edge-Cache-Tag";                     

        readonly ILog _logger = LogManager.GetLogger("CacheTaggedController");
        protected Func<IEnumerable<string>, IEnumerable<string>> CacheTagFormatter = tags =>
        {
            return tags;
        };              
        protected virtual void AddCacheTagsHeader(HttpContextBase httpContext, IEnumerable<string> tags)
        {
            try
            {            
                if (_enableCacheTag && tags.Any())
                {
                    IEnumerable<string> cacheTagsHeader = new List<string> { };

                    if (!httpContext.Response.Headers.AllKeys.Any(x => x.Equals(_edgeCacheTag)))
                    {                   
                        httpContext.Response.Headers.Add(_edgeCacheTag, string.Join(",", CacheTagFormatter(tags)));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error generating CacheTag: {string.Join(",", tags)}", ex);
                throw new Exception("Error generating CacheTag");
            }

        }
    
    }

}

