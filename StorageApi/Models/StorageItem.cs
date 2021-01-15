using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageApi.Models
{
    public class StorageItem
    {
        public StorageItem(string itemName)
        {
            ItemName = itemName;
        }

        public string ItemName { get; }

        public int AvailableItemsCount { get; set; }
    }
}
