using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Exceptions;
using StorageApi.Interfaces;
using StorageApi.Models;

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
            var item = GetItemByName(itemName);
            if (item is null)
                throw new NotFoundException("Could not find an item matching the request.");
            return item.ItemAmount;
        }

        public void AddItemAmount(string itemName, int amountToAdd)
        {
            var itemToUpdate = GetItemByName(itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            if (itemToUpdate.ItemAmount + amountToAdd > 500)
                throw new FullStorageException("Item amount for requested item exceeded max storage capacity.");
            _storage.StoredItems[_storage.StoredItems.IndexOf(itemToUpdate)].ItemAmount += amountToAdd;
        }

        public void RemoveItemAmount(string itemName, int amountToRemove)
        {
            var itemToUpdate = GetItemByName(itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            var itemIndex = _storage.StoredItems.IndexOf(itemToUpdate);
            if (itemToUpdate.ItemAmount - amountToRemove < 0)
                throw new EmptyStorageException("Item amount for requested item cannot be negative.");
            _storage.StoredItems[itemIndex].ItemAmount -= amountToRemove;
        }

        public void AddItemAmountToAll()
        {
            if(_storage.StoredItems.Any(item => item.ItemAmount + 10 > 500))
                throw new FullStorageException($"Item amount exceeded max storage capacity.");
            foreach (var item in _storage.StoredItems)
            {
                item.ItemAmount += 10;
            }
        }

        public IEnumerable<StorageItem> GetAllItems()
        {
            return _storage.StoredItems;
        }

        public void RemoveMany(List<StorageItem> items)
        {
            foreach (var item in items)
            {
                var storedItem = GetItemByName(item?.ItemName);
                if(storedItem is null)
                    throw new NotFoundException("Could not find an item matching the request.");
                if(storedItem.ItemAmount - item?.ItemAmount < 0)
                    throw new EmptyStorageException("Item amount for requested item cannot be negative.");
            }
            foreach (var item in items)
            {
                var storedItem = GetItemByName(item.ItemName);
                _storage.StoredItems[_storage.StoredItems.IndexOf(storedItem)].ItemAmount -= item.ItemAmount;
            }
        }

        private StorageItem GetItemByName(string itemName)
        {
            return _storage.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
        }
    }
}
