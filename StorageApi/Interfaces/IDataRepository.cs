﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Models;

namespace StorageApi.Interfaces
{
    public interface IDataRepository
    {
        int GetItemCount(string itemName);
        
        void AddItemAmount(string itemName, int amountToAdd);

        void RemoveItemAmount(string itemName, int amountToRemove);

        void AddItemAmountToAll();

        IEnumerable<StorageItem> GetAllItems();
    }
}