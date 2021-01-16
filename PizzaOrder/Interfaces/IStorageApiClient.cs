using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaOrder.Models;

namespace PizzaOrder.Interfaces
{
    public interface IStorageApiClient
    {
        Task RemoveFromStorage(Order order);
    }
}
