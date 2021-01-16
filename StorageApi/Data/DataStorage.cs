using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class DataStorage : IDataStorage
    {
        public DataStorage()
        {
            StoredItems = new List<StorageItem>
            {
                new StorageItem("Ost"){ ItemAmount = 0},
                new StorageItem("Tomatsås"){ ItemAmount = 0},
                new StorageItem("Skinka"){ ItemAmount = 0},
                new StorageItem("Ananas"){ ItemAmount = 0},
                new StorageItem("Kebab"){ ItemAmount = 0},
                new StorageItem("Champinjoner"){ ItemAmount = 0},
                new StorageItem("Lök"){ ItemAmount = 0},
                new StorageItem("Feferoni"){ ItemAmount = 0},
                new StorageItem("Isbergssallad"){ ItemAmount = 0},
                new StorageItem("Tomat"){ ItemAmount = 0},
                new StorageItem("Kebabsås"){ ItemAmount = 0},
                new StorageItem("Räkor"){ ItemAmount = 0},
                new StorageItem("Musslor"){ ItemAmount = 0},
                new StorageItem("Kronärtskocka"){ ItemAmount = 0},
                new StorageItem("Coca cola"){ ItemAmount = 0},
                new StorageItem("Fanta"){ ItemAmount = 0},
                new StorageItem("Sprite"){ ItemAmount = 0},
            };
        }

        public List<StorageItem> StoredItems { get; set; }
    }
}
