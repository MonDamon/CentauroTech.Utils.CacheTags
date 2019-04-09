using FluentAssertions;    
using Xunit;
using CentauroTech.Utils.CacheTags;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace CentauroTech.Utils.CacheTags.Tests.UnitTests
{    
    public class CacheTagsTests
    {
        [Fact]       
        public void Returns_no_header_when_emptylist_is_passed()
        {
            //arrange
            var collection = new NameValueCollection();

            collection.Add(new List<string>{},"","namedCollection");

            //assert    
            collection.Should().BeEmpty();        
        }

         [Fact]       
        public void Returns_header_when_noemptylist_is_passed()
        {
            //arrange
            var collection = new NameValueCollection();

            collection.Add(new List<string>{"tag"},"","namedCollection");

            //assert    
            collection.Should().NotBeEmpty();        
        }
            
    }
}