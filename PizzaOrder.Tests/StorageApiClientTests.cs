using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PizzaOrder.Integrations;
using PizzaOrder.Models;
using PizzaOrder.Tests.Fakes;

namespace PizzaOrder.Tests
{
    [TestClass]
    public class StorageApiClientTests
    {
        [TestMethod]
        public async Task Remove_ingredients_Should_succeed_without_throwing_exceptions()
        {
            var baseUrl = "http://localhost:60479/";
            var url = "api/StorageItem";
            var orderables = new List<IOrderable>()
            {
                new Pizza(
                    "testPizza", 
                    1, 
                    new [] {"ost"}, 
                    new List<IAddable>()
                    {
                        new Topping("Tomat", 1)
                    }),
                new Pizza(
                    "testPizza", 
                    1, 
                    new [] {"ost"}, 
                    new List<IAddable>()
                    {
                        new Topping("Tomat", 1)
                    }),
                new Drink("Fanta", 1)
            };
            var order = new Order(1, orderables);
            var Items = new List<StorageItem>()
            {
                new StorageItem("Ost") {ItemAmount = 2},
                new StorageItem("Tomat") {ItemAmount = 2},
                new StorageItem("Fanta") {ItemAmount = 1}
            };
            var jsonString = JsonConvert.SerializeObject(Items);
            var message = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(jsonString)
            };
            var storageClient =
                new StorageApiClient(GetDefaultConfiguration(), CreateTestClient(baseUrl, url, message));
            await storageClient.RemoveFromStorage(order);
        }
        // Exception_If_response_content_cannot_be_deserialized()
        [TestMethod]
        public async Task Remove_ingredients_Should_throw_HttpRequestException_If_client_returns_unsuccessful_status_code()
        {
            var baseUrl = "http://localhost:60479/";
            var url = "api/StorageItem";
            var orderables = new List<IOrderable>()
            {
                new Pizza(
                    "testPizza", 
                    1, 
                    new [] {"ost"}, 
                    new List<IAddable>()
                    {
                        new Topping("Tomat", 1)
                    }),
                new Drink("Fanta", 1)
            };
            var order = new Order(1, orderables);
            var Items = new List<StorageItem>()
            {
                new StorageItem("Ost") {ItemAmount = 1},
                new StorageItem("Tomat") {ItemAmount = 1},
                new StorageItem("Fanta") {ItemAmount = 1}
            };
            var jsonString = JsonConvert.SerializeObject(Items);
            var message = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest, 
                Content = new StringContent(jsonString)
            };
            var storageClient =
                new StorageApiClient(GetDefaultConfiguration(), CreateTestClient(baseUrl, url, message));
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => storageClient.RemoveFromStorage(order));
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
            var client = new HttpClient(new TestHttpMessageHandler(requests));
            return client;
        }

        private IConfiguration GetDefaultConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"StorageApiUrl", "http://localhost:60479/"}
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
    }
}
