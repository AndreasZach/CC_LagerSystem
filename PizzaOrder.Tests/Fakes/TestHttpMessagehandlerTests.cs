using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PizzaOrder.Tests.Fakes
{
    [TestClass]
    public class TestHttpMessageHandlerTests
    {
        [TestMethod]
        public async Task If_url_cannot_be_found_Should_throw_exception()
        {
            // Arrange
            var baseUri = "http://localhost:60479/";
            var url = "Storage/RemoveItemAmount";
            var jsonString = JsonConvert.SerializeObject(1);
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonString)
            };
            var client = CreateTestClient(baseUri, url, httpResponseMessage);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => client.GetAsync("wrongUrl"));
        }

        [TestMethod]
        public async Task If_url_cannot_be_found_Should_return_expected_result()
        {
            // Arrange
            var baseUri = "http://localhost:60479/";
            var url = "Storage/RemoveItemAmount";
            var jsonString = JsonConvert.SerializeObject(1);
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonString)
            };
            var client = CreateTestClient(baseUri, url, httpResponseMessage);

            // Act
            var result = await client.GetAsync(url);

            // Assert
            Assert.AreEqual(200, (int)result.StatusCode);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.AreEqual(jsonString, resultContent);
        }

        private HttpClient CreateTestClient(string baseUri, string url, HttpResponseMessage httpResponseMessage = null)
        {
            var requests = new Dictionary<string, HttpResponseMessage>
            {
                {
                    baseUri + url, 
                    httpResponseMessage
                }
            };
            var client = new HttpClient(new TestHttpMessageHandler(requests))
            {
                BaseAddress = new Uri(baseUri)
            };
            return client;
        }
    }
}
