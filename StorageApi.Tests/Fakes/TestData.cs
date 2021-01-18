using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageApi.Models;

namespace StorageApi.Tests.Fakes
{
    class TestData
    {
        public IEnumerable<StorageItem> GetDefaultTestData(int itemAmountPerItem)
        {
            return  new List<StorageItem>
            {
                new StorageItem("Ost"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Tomatsås"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Skinka"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Ananas"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Kebab"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Champinjoner"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Lök"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Feferoni"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Isbergssallad"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Tomat"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Kebabsås"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Räkor"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Musslor"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Kronärtskocka"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Coca cola"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Fanta"){ ItemAmount = itemAmountPerItem},
                new StorageItem("Sprite"){ ItemAmount = itemAmountPerItem},
            };
        }
    }
}
