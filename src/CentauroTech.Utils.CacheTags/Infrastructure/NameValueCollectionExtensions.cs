
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;



namespace CentauroTech.Utils.CacheTags
{

    public static class NameValueCollectionExtensions
    {        
        public static void Add(this NameValueCollection collection, IEnumerable<string> tags, string prefix = "",string collectionName = "")
        {
            IEnumerable<string> cacheTagsHeader = new List<string> { };

            cacheTagsHeader = cacheTagsHeader.Concat(tags.Select(x => $"{(string.IsNullOrWhiteSpace(prefix) ? prefix : $"{prefix}-")}{x}"));
            
            if(!cacheTagsHeader.Any())
                return;

            collection.Add(collectionName, String.Join(",", cacheTagsHeader));
        }
        public static void Add(this NameValueCollection collection, IEnumerable<string> tags, Func<IEnumerable<string>, IEnumerable<string>> formatter, string collectionName  = "")
        {
            Add(collection:collection, tags:formatter(tags),collectionName:collectionName);
        }
    }
}
