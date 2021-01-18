using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageApi.Exceptions;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class DataRepository : IDataRepository
    {
        private StorageItemContext _context;

        public DataRepository(StorageItemContext context)
        {
            _context = context;
        }

        public async Task<int> GetItemCountAsync(string itemName)
        {
            var item = await GetItemByNameAsync(itemName);
            if (item is null)
                throw new NotFoundException("Could not find an item matching the request.");
            return item.ItemAmount;
        }

        public async Task AddItemAmountAsync(string itemName, int amountToAdd)
        {
            var itemToUpdate = await GetItemByNameAsync(itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            if (itemToUpdate.ItemAmount + amountToAdd > 500)
                throw new FullStorageException("Item amount for requested item exceeded max storage capacity.");
            itemToUpdate.ItemAmount += amountToAdd;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAmountAsync(string itemName, int amountToRemove)
        {
            var itemToUpdate = await GetItemByNameAsync(itemName);
            if (itemToUpdate is null)
                throw  new NotFoundException("Could not find an item matching the request.");
            if (itemToUpdate.ItemAmount - amountToRemove < 0)
                throw new EmptyStorageException("Item amount for requested item cannot be negative.");
            itemToUpdate.ItemAmount -= amountToRemove;
            await _context.SaveChangesAsync();
        }

        public async Task AddItemAmountToAllAsync()
        {
            if(_context.StorageItems.Any(item => item.ItemAmount + 10 > 500))
                throw new FullStorageException($"Item amount exceeded max storage capacity.");
            foreach (var item in await GetAllItemsAsync())
            {
                item.ItemAmount += 10;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StorageItem>> GetAllItemsAsync()
        {
            return await _context.StorageItems.ToListAsync();
        }

        public async Task RemoveManyAsync(List<StorageItem> items)
        {
            foreach (var item in items)
            {
                var storedItem = await GetItemByNameAsync(item?.ItemName);
                if(storedItem is null)
                    throw new NotFoundException("Could not find an item matching the request.");
                if(storedItem.ItemAmount - item?.ItemAmount < 0)
                    throw new EmptyStorageException("Item amount for requested item cannot be negative.");
            }
            foreach (var item in items)
            {
                var storedItem = await GetItemByNameAsync(item.ItemName);
                storedItem.ItemAmount -= item.ItemAmount;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<StorageItem> GetItemByNameAsync(string itemName)
        {
            return await _context.StorageItems.FirstOrDefaultAsync(x => x.ItemName == itemName);
        }
    }
}
