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
using System;
using System.Configuration;

namespace CentauroTech.Utils.CacheTags.Tests.UnitTests
{
    public class CacheTagsTests : IDisposable
    {
        private const string ENABLECACHETAG = "EnableCacheTag";
        private const string URLTOACCESS = "http://localhost/api/test?query=123";

        [Fact]
        public void Returns_header_when_enablecachetag_istrue()
        {
            ConfigurationManager.AppSettings[ENABLECACHETAG] = "true";
            var controller = new TestTaggedController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, URLTOACCESS); 

            var route = config.Routes.MapHttpRoute("Test",
                "test",
                new { controller = "TestTagged", action = "Test" }
            );

            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "TestTagged", "Test" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

           // var response = controller.ExecuteAsync ExecuteAsync

          //  response.Headers.Any().Should().BeTrue();

        }

        [Fact]
        public void Returns_no_header_when_enablecachetag_isfalse()
        {
            ConfigurationManager.AppSettings[ENABLECACHETAG] = "false";
            var controller = new TestTaggedController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, URLTOACCESS);

            var route = config.Routes.MapHttpRoute("Test",
                "test",
                new { controller = "TestTagged", action = "Test" }
            );

            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "TestTagged", "Test" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
         
            var response =  request.CreateResponse(HttpStatusCode.OK, new List<string> { });

            response.Headers.Any(x => x.Key == "Edge-Cache-Tag").Should().BeFalse();

        }

        public void Dispose() => ConfigurationManager.AppSettings[ENABLECACHETAG] = null;
    }

    public class TestTaggedController : CacheTaggedApiController
    {

        [CacheTaggable("query")]
        public HttpResponseMessage Test(string query) => Request.CreateResponse(HttpStatusCode.OK, true);

    }

}