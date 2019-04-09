
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Linq;


namespace CentauroTech.Utils.CacheTags
{ 
    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage AddCacheTagsHeader(this HttpResponseMessage response, IEnumerable<string> tags,string prefix = "")
        {
            var edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTagName"] ?? "Edge-Cache-Tag";           
            
            if (!response.Headers.TryGetValues(edgeCacheTag, out IEnumerable<string> cacheTagsHeader))            
                cacheTagsHeader = new List<string> { };
            
            cacheTagsHeader = cacheTagsHeader.Concat(tags.Select(x=>$"{(string.IsNullOrWhiteSpace(prefix)? prefix: $"{prefix}-" )}{x}"));

            if(cacheTagsHeader.Any())
                return response;

            response.Headers.Add(edgeCacheTag, string.Join(",", cacheTagsHeader));

            return response;
        }
        public static HttpResponseMessage AddCacheTagsHeader(this HttpResponseMessage response, IEnumerable<string> tags,  Func<IEnumerable<string>, IEnumerable<string>> formatter)
        {
            return AddCacheTagsHeader(response,formatter(tags));
        }
    }
}