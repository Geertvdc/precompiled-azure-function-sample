using PreCompiledFunctionDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Xunit;

namespace PreCompiledFunctionSample.Tests
{
    public class SampleFunctionTests
    {
        [Theory]
        [InlineData("Geert")]
        [InlineData("John")]
        public void IfQueryStringParameterNameFunctionReturnHelloName(string name)
        {
            //Arrange
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,"http://testfunction?name=" + name);
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            //Act
            var response = NameFunction.Run(req).Result;

            //Assert
            Assert.Equal("\"Hello " +name +"\"", response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void NoParametersReturnsHelpMessage()
        {
            //Arrange
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "http://testfunction");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            //Act
            var response = NameFunction.Run(req).Result;

            //Assert
            Assert.Equal("\"Please pass a name on the query string or in the request body\"", response.Content.ReadAsStringAsync().Result);

        }

        [Theory]
        [InlineData("Geert")]
        [InlineData("John")]
        public void IfRequestBodyContainsNameFunctionReturnHelloName(string name)
        {
            //Arrange
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "http://testfunction");
            req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            req.Content = new StringContent("{\"name\":\"" + name + "\"}", Encoding.UTF8, "application/json");
            

            //Act
            var response = NameFunction.Run(req).Result;

            //Assert
            Assert.Equal("\"Hello " + name + "\"", response.Content.ReadAsStringAsync().Result);
        }


    }
}
