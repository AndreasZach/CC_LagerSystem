﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaOrder.Interfaces;

namespace PizzaOrder.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class OrdersController : Controller {
        private OrderableController orderableController = new OrderableController();
        private List<Order> orders;
        private IStorageApiClient client;
        private Storage _storage;
        private List<IAddable> addables = new List<IAddable>() {
            new Topping("Skinka", 10),
            new Topping("Ananas", 10),
            new Topping("Champinjoner", 10),
            new Topping("Lök", 10),
            new Topping("Kebabsås", 10),
            new Topping("Räkor", 15),
            new Topping("Musslor", 15),
            new Topping("Kronärtskocka", 15),
            new Topping("Kebab", 20),
            new Topping("Koriander", 20)
        };

        public OrdersController(Storage storage, IStorageApiClient client) {
            orders = storage.data;
            _storage = storage;
            this.client = client;
        }

        [HttpPost]
        public Order Create(IEnumerable<string> itemNames) {
            var items = itemNames.Select(x => orderableController.GetOrderable(x)).ToList();
            var order = new Order(_storage.Index, items);
            orders.Add(order);
            _storage.Index++;
            return order;
        }

        [HttpDelete]
        public void RemoveItemFromOrder(int orderId, string itemName) {
            var order = Get(orderId);
            var item = order.Items.Where(x => x.Name == itemName).FirstOrDefault();
            if (item == null) throw new ArgumentNullException("No such item");
            order.Items.Remove(item);
        }

        [HttpGet]
        public Order Get(int orderId) {
            return orders.Where(x => x.Id == orderId).FirstOrDefault();
        }

        [HttpPut("/AddItemToOrder")]
        public void AddItemToOrder(int orderId, string itemName) {
            var order = Get(orderId);
            var item = orderableController.GetOrderable(itemName);
            order.Items.Add(item);
        }

        [HttpPut("/AddAddable")]
        public void AddAddable(int orderId, int itemIndex, string addableName) {
            var order = Get(orderId);
            IAddable addable = GetAddable(addableName);
            if (addable == null) throw new ArgumentNullException("No such addable");
            (order.Items[itemIndex] as Pizza).Addables.Add(addable);
        }

        [HttpGet("/GetAddable")]
        private IAddable GetAddable(string addableName) {
            return addables.Where(x => x.Name == addableName).FirstOrDefault();
        }

        [HttpDelete("/RemoveAddable")]
        public void RemoveAddable(int orderId, int itemIndex, string addableName) {
            var order = Get(orderId);
            var addable = GetAddable(addableName);
            if (addable == null) throw new ArgumentNullException("No such addable");
            (order.Items[itemIndex] as Pizza).Addables.Remove(addable);
        }

        [HttpPut("/Confirm")]
        public async Task<Order> Confirm(int orderId) {
            var order = Get(orderId);
            if (order.Status == Order.OrderStatus.Created)
                order.Status = Order.OrderStatus.Confirmed;
            else {
                throw new InvalidOperationException("Can only confirm orders with OrderStatus 'Created'");
            }

            await client.RemoveFromStorage(orders.Find(x => x.Id == orderId));
            return order;
        }

        [HttpGet("/GetActive")]
        public IEnumerable<Order> GetActive() {
            return orders.Where(x => x.Status == Order.OrderStatus.Confirmed).ToList();
        }

        [HttpPut("/Cancel")]
        public void Cancel(int orderId) {
            var order = Get(orderId);
            if (order.Status == Order.OrderStatus.Confirmed || order.Status == Order.OrderStatus.Created) {
                order.Status = Order.OrderStatus.Cancelled;
            }
            else {
                throw new InvalidOperationException("Can only cancel orders with OrderStatus 'Created' or 'Confirmed'");
            }
        }

        [HttpPut("/Complete")]
        public void Complete(int orderId) {
            var order = Get(orderId);
            if (order.Status == Order.OrderStatus.Confirmed) {
                order.Status = Order.OrderStatus.Completed;
            }
            else {
                throw new InvalidOperationException("Can only confirm orders with OrderStatus 'Confirmed'");
            }
        }
        
        [HttpDelete("/Delete")]
        public void DeleteOrder(int orderId)
        {
            var order = Get(orderId);
            if(order is not null)
                _storage.data.Remove(order);
        }
    }
}
