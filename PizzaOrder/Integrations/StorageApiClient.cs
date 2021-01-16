using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PizzaOrder.Interfaces;
using PizzaOrder.Models;

namespace PizzaOrder.Integrations
{
    public class StorageApiClient : IStorageApiClient
    {
        private readonly HttpClient _client;
        private readonly string _url = "api/StorageItem";

        public StorageApiClient(IConfiguration config, HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(config.GetValue<string>("StorageApiUrl"));
        }


        public async Task RemoveFromStorage(Order order)
        {
            var items = GetAllItemsFromOrder(order);
            var groupedItems = GroupItemsByName(items).ToList();
            HttpResponseMessage result;
            try
            {
                var jsonString = JsonConvert.SerializeObject(groupedItems);
                result = await _client.PostAsync(_url, new StringContent(jsonString, Encoding.UTF8, "application/json"));
            }
            catch (Exception e) {
                throw new Exception("Post to StorageApi failed", e);
            }
            if (!result.IsSuccessStatusCode) {
                throw new HttpRequestException($"Remove items from storage HttpRequest failed with code {result.StatusCode}");
            }
        }

        private IEnumerable<StorageItem> GetAllItemsFromOrder(Order order)
        {
            var items = new List<StorageItem>();
            foreach (var item in order.Items)
            {
                if (item is Drink)
                {
                    items.Add(new StorageItem(item.Name) {ItemAmount = 1});
                }
                else
                {
                    items.AddRange((item as Pizza).Toppings
                        .Select(topping => new StorageItem(topping) {ItemAmount = 1}));
                    items.AddRange((item as Pizza).Addables
                        .Select(addable => new StorageItem(addable.Name) {ItemAmount = 1}));
                }
            }
            return items;
        }

        private IEnumerable<StorageItem> GroupItemsByName(IEnumerable<StorageItem> items)
        {
            return items
                .GroupBy(x => x.ItemName)
                .Select(g => new StorageItem(g.Key) {ItemAmount = g.Sum(s => s.ItemAmount)});
        }
    }
}
