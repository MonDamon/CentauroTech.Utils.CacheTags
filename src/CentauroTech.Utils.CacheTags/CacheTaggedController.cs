
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
        readonly bool _habilitarCacheTag = Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitarCacheTag"] ?? "false");
        readonly string _edgeCacheTag = ConfigurationManager.AppSettings["EdgeCacheTag"] ?? "Edge-Cache-Tag";                     
        private Func<IEnumerable<string>, IEnumerable<string>> MontarCacheTag = tags =>
        {

            string tipoTagModelo = ConfigurationManager.AppSettings["TipoCacheTagModelo"] ?? "data-model";
            string prefixo = ConfigurationManager.AppSettings["PrefixoCacheTag"];

            return new List<string> { }.Concat(tags
                    .Select(x => $"{(string.IsNullOrWhiteSpace(tipoTagModelo) ? tipoTagModelo : $"{tipoTagModelo}-")}{x}")
                        .Select(z => $"{(string.IsNullOrWhiteSpace(prefixo) ? prefixo : $"{prefixo}-")}{z}")).Distinct();

        };

        protected virtual void AddCacheTagsHeader(HttpContextBase httpContext, IEnumerable<string> tags, string prefixo = "")
        {

            if (_habilitarCacheTag && tags.Any())
            {
                IEnumerable<string> cacheTagsHeader = new List<string> { };

                if (!httpContext.Response.Headers.AllKeys.Any(x => x.Equals(_edgeCacheTag)))
                {
                    cacheTagsHeader = cacheTagsHeader.Concat(tags.Select(x => $"{(string.IsNullOrWhiteSpace(prefixo) ? prefixo : $"{prefixo}-")}{x}"));

                    var cacheTagsHeaderFormatado = MontarCacheTag(cacheTagsHeader);

                    httpContext.Response.Headers.Add(_edgeCacheTag, string.Join(",", cacheTagsHeaderFormatado));
                }
            }

        }
    
    }

}

