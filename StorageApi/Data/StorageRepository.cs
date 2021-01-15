using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class StorageRepository : IStorageRepository
    {
        public StorageRepository()
        {
            StoredItems = new List<StorageItem>
            {
                new StorageItem("Ost"){ AvailableItemsCount = 0},
                new StorageItem("Tomatsås"){ AvailableItemsCount = 0},
                new StorageItem("Skinka"){ AvailableItemsCount = 0},
                new StorageItem("Ananas"){ AvailableItemsCount = 0},
                new StorageItem("Kebab"){ AvailableItemsCount = 0},
                new StorageItem("Champinjoner"){ AvailableItemsCount = 0},
                new StorageItem("Lök"){ AvailableItemsCount = 0},
                new StorageItem("Feferoni"){ AvailableItemsCount = 0},
                new StorageItem("Isbergssallad"){ AvailableItemsCount = 0},
                new StorageItem("Tomat"){ AvailableItemsCount = 0},
                new StorageItem("Kebabsås"){ AvailableItemsCount = 0},
                new StorageItem("Räkor"){ AvailableItemsCount = 0},
                new StorageItem("Musslor"){ AvailableItemsCount = 0},
                new StorageItem("Kronärtskocka"){ AvailableItemsCount = 0},
                new StorageItem("Coca cola"){ AvailableItemsCount = 0},
                new StorageItem("Fanta"){ AvailableItemsCount = 0},
                new StorageItem("Sprite"){ AvailableItemsCount = 0},
            };
        }

        public List<StorageItem> StoredItems { get; set; }
    }
}
