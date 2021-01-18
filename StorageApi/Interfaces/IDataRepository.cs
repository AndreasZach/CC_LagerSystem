using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Models;

namespace StorageApi.Interfaces
{
    public interface IDataRepository
    {
        Task<int> GetItemCountAsync(string itemName);
        
        Task AddItemAmountAsync(string itemName, int amountToAdd);

        Task RemoveItemAmountAsync(string itemName, int amountToRemove);

        Task AddItemAmountToAllAsync();

        Task<IEnumerable<StorageItem>> GetAllItemsAsync();

        Task RemoveManyAsync(List<StorageItem> items);
    }
}
