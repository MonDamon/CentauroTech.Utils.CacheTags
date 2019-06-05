using FluentAssertions;
using Xunit;


using System.Web.Http.Controllers;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using System.Net;
using System.Collections.Generic;

namespace CentauroTech.Utils.CacheTags.Tests.UnitTests
{
    public class CacheTagsTests
    {
     

        [Fact]
        public void Returns_no_header_when_addcachetag_isfalse()
        {
            var controller = new CacheTaggedApiController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test?query=123");
            var route = config.Routes.MapHttpRoute("default", "api/{controller}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "test" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
         
            var response =  request.CreateResponse(HttpStatusCode.OK, new List<string> { });

            response.Headers.Any(x => x.Key == "Edge-Cache-Tag").Should().BeFalse();

        }

    }
}