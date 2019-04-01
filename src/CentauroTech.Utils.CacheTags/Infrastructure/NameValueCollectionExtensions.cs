
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;



namespace ByTennis.Core.Extensions
{

    public static class NameValueCollectionExtensions
    {
        static readonly string edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTagName"] ?? "Edge-Cache-Tag";       
        public static void AddCacheTagsHeader(this NameValueCollection collection, IEnumerable<string> tags, string prefix = "")
        {
            IEnumerable<string> cacheTagsHeader = new List<string> { };

            cacheTagsHeader = cacheTagsHeader.Concat(tags.Select(x => $"{(string.IsNullOrWhiteSpace(prefix) ? prefix : $"{prefix}-")}{x}"));

            collection.Add(edgeCacheTag, String.Join(",", cacheTagsHeader));

        }
        public static void AddCacheTagsHeader(this NameValueCollection response, IEnumerable<string> tags, Func<IEnumerable<string>, IEnumerable<string>> formatter)
        {
            AddCacheTagsHeader(response, formatter(tags));
        }
    }
}
