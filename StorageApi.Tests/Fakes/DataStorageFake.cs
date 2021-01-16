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
                new StorageItem("Ost"){ ItemAmount = availableItemCount},
                new StorageItem("Tomatsås"){ ItemAmount = availableItemCount},
                new StorageItem("Skinka"){ ItemAmount = availableItemCount},
                new StorageItem("Ananas"){ ItemAmount = availableItemCount},
                new StorageItem("Kebab"){ ItemAmount = availableItemCount},
                new StorageItem("Champinjoner"){ ItemAmount = availableItemCount},
                new StorageItem("Lök"){ ItemAmount = availableItemCount},
                new StorageItem("Feferoni"){ ItemAmount = availableItemCount},
                new StorageItem("Isbergssallad"){ ItemAmount = availableItemCount},
                new StorageItem("Tomat"){ ItemAmount = availableItemCount},
                new StorageItem("Kebabsås"){ ItemAmount = availableItemCount},
                new StorageItem("Räkor"){ ItemAmount = availableItemCount},
                new StorageItem("Musslor"){ ItemAmount = availableItemCount},
                new StorageItem("Kronärtskocka"){ ItemAmount = availableItemCount},
                new StorageItem("Coca cola"){ ItemAmount = availableItemCount},
                new StorageItem("Fanta"){ ItemAmount = availableItemCount},
                new StorageItem("Sprite"){ ItemAmount = availableItemCount},
            };
        }
        public List<StorageItem> StoredItems { get; set; }
    }
}
