
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
        private const string EDCACHETAG = "EdgeCacheTag";
        private const string CACHETAGGEDAPICONTROLLER = "CacheTaggedApiController";
        private const string EDGECACHETAGVALUE = "Edge-Cache-Tag";
        private readonly string _edgeCacheTag = ConfigurationManager.AppSettings[EDCACHETAG] ?? EDGECACHETAGVALUE;
     
        public IEnumerable<string> QueryStringToCheck { get; set; } = new List<string>();
        public bool AddCacheTag { get; set; } = true;

        protected Func<IEnumerable<string>, IEnumerable<string>> CacheTagFormatter = tags =>
        {
            return tags;
        };

        protected virtual HttpResponseMessage AddCacheTagsHeader(HttpResponseMessage response, IEnumerable<string> tags)
        {
            if (AddCacheTag && tags.Any())
            {

                if (!response.Headers.TryGetValues(_edgeCacheTag, out IEnumerable<string> cacheTagsHeader))
                {
                    cacheTagsHeader = new List<string> { };
                }

                cacheTagsHeader = cacheTagsHeader.Concat(tags);

                if (cacheTagsHeader.Any())
                {
                    response.Headers.Add(_edgeCacheTag, string.Join(",", cacheTagsHeader));
                }
            }
            return response;

        }

        private List<string> GetTagList(string query)
        {

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
                            if (!new List<string> { }.Any(x => x.Equals(paramValue)))
                            {
                                new List<string> { }.Add(paramValue);
                            }
                        }
                    }
                }

            }

            return new List<string> { };
        }

        protected IEnumerable<string> CacheTagList(IEnumerable<string> tags) => CacheTagFormatter(tags);

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(controllerContext, cancellationToken).ContinueWith((x) =>
            {
                if (AddCacheTag && GetTagList(controllerContext.Request.RequestUri.Query).Any())
                {
                    return AddCacheTagsHeader(x.Result, CacheTagList(GetTagList(controllerContext.Request.RequestUri.Query)));
                }

                return x.Result;
            }
            );
        }

    }

}

