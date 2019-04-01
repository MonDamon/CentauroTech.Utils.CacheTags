
using ByTennis.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Linq;


namespace Centauro.Web.Framework.Extensions
{ 
    public static class HttpRequestMessageExtensions
    {
        static string edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTagName"] ?? "Edge-Cache-Tag";           
        public static HttpResponseMessage AddCacheTagsHeader(this HttpResponseMessage response, IEnumerable<string> tags,string prefix = "")
        {
            if (!response.Headers.TryGetValues(EdgeCacheTag, out IEnumerable<string> cacheTagsHeader))            
                cacheTagsHeader = new List<string> { };

            cacheTagsHeader = cacheTagsHeader.Concat(tags.Select(x=>$"{(string.IsNullOrWhiteSpace(prefix)? prefix: $"{prefix}-" )}{x}"));

            response.Headers.Add(edgeCacheTag, string.Join(",", cacheTagsHeader));

            return response;
        }
        public static HttpResponseMessage AddCacheTagsHeader(this HttpResponseMessage response, IEnumerable<string> tags,  Func<IEnumerable<string>, IEnumerable<string>> formatter)
        {
            return AddCacheTagsHeader(response,formatter(tags));
        }
    }
}