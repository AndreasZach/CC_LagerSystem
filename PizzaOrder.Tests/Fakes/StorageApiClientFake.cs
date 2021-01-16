using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaOrder.Interfaces;

namespace PizzaOrder.Tests.Fakes
{
    class StorageApiClientFake : IStorageApiClient
    {
        public async Task RemoveFromStorage(Order order)
        {
            await Task.Delay(100);
        }
    }
}
