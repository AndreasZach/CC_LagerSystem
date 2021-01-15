using System;
using System.Collections.Generic;
using System.Text;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Tests.Fakes
{
    class DataStorageFake : IDataStorage
    {
        public DataStorageFake(int availableItemCount = 0)
        {
            StoredItems = new List<StorageItem>
            {
                new StorageItem("Ost"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Tomatsås"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Skinka"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Ananas"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Kebab"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Champinjoner"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Lök"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Feferoni"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Isbergssallad"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Tomat"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Kebabsås"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Räkor"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Musslor"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Kronärtskocka"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Coca cola"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Fanta"){ AvailableItemsCount = availableItemCount},
                new StorageItem("Sprite"){ AvailableItemsCount = availableItemCount},
            };
        }
        public List<StorageItem> StoredItems { get; set; }
    }
}
