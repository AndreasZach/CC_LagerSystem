using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Exceptions;
using StorageApi.Interfaces;

namespace StorageApi.Data
{
    public class DataRepository : IDataRepository
    {
        private IDataStorage _storage;

        public DataRepository(IDataStorage storage)
        {
            _storage = storage;
        }

        public int GetItemCount(string itemName)
        {
            var item = _storage.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
            if (item is null)
                throw new NotFoundException("Could not find an item matching the request.");
            return item.AvailableItemsCount;
        }

        public void AddItemAmount(string itemName, int amountToAdd)
        {
            var itemToUpdate = _storage.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            if (itemToUpdate.AvailableItemsCount + amountToAdd > 500)
                throw new FullStorageException("Item amount for requested item exceeded max storage capacity.");
            _storage.StoredItems[_storage.StoredItems.IndexOf(itemToUpdate)].AvailableItemsCount += amountToAdd;
        }

        public void RemoveItemAmount(string itemName, int amountToRemove)
        {
            var itemToUpdate = _storage.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            var itemIndex = _storage.StoredItems.IndexOf(itemToUpdate);
            if (itemToUpdate.AvailableItemsCount - amountToRemove < 0)
                throw new EmptyStorageException("Item amount for requested item cannot be negative.");
            _storage.StoredItems[itemIndex].AvailableItemsCount -= amountToRemove;
        }
    }
}
