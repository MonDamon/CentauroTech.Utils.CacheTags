
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

namespace CentauroTech.Utils.CacheTags
{ 
    public class CacheTaggedApiController : ApiController
    {            
        readonly string _edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTag"] ?? "Edge-Cache-Tag";
        readonly ILog _logger = LogManager.GetLogger("CacheTaggedApiController");
        public IEnumerable<string> QueryStringToCheck { get; set; } = new List<string> ();

        public bool AddCacheTag { get; set; }      
        protected Func<IEnumerable<string>, IEnumerable<string>> CacheTagFormatter = tags =>
        {
            return tags;
        };

        protected virtual HttpResponseMessage AddCacheTagsHeader(HttpResponseMessage response, IEnumerable<string> tags)
        {
            try
            {
                if (AddCacheTag && tags.Any())
                {

                    if (!response.Headers.TryGetValues(_edgeCacheTag, out IEnumerable<string> cacheTagsHeader))
                        cacheTagsHeader = new List<string> { };

                    cacheTagsHeader = cacheTagsHeader.Concat(tags);

                    if (cacheTagsHeader.Any())
                        response.Headers.Add(_edgeCacheTag, string.Join(",", cacheTagsHeader));
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error generating CacheTag: {string.Join(",", tags)}", ex);
                throw new Exception("Error generating CacheTag");
            }

        }

        private List<string> GetTagList(string query)
        {
            var result = new List<string> { };

            var urlParameters = HttpUtility.ParseQueryString(query);

            foreach (var parName in QueryStringToCheck)
            {
                if (urlParameters.AllKeys.Contains(parName, new UrlParameterComparer { }))
                {
                    var paramComplete = urlParameters.Get(parName);
                    if (!string.IsNullOrWhiteSpace(paramComplete) && !paramComplete.Split(',').Any(x => x.Length < 6))
                    {

                        var paramValues = paramComplete.Split(',').Select(x => x.Substring(0, 6));
                        foreach (var paramValue in paramValues)
                        {
                            if (!result.Any(x => x.Equals(paramValue)))
                                result.Add(paramValue);
                        }
                    }
                }

            }

            return result;
        }

        protected  IEnumerable<string> CacheTagList( IEnumerable<string> tags)
        {
            
            return CacheTagFormatter(tags);
        }  
           
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(controllerContext, cancellationToken).ContinueWith((x) =>
            {               
                    if (AddCacheTag)
                    {
                        var cacheTagHeader = GetTagList(controllerContext.Request.RequestUri.Query);

                        if (cacheTagHeader.Any())
                        {
                            return AddCacheTagsHeader(x.Result, CacheTagList(cacheTagHeader));
                        }
                    }                
                return x.Result;
            }
            );
        }
    
    }

}

