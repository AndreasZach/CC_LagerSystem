using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageApi.Data;

namespace StorageApi.Tests
{
    [TestClass]
    public class DataStorageTests
    {
        [DataTestMethod]
        [DataRow("Ost", 0)]
        [DataRow("Tomatsås", 0)]
        [DataRow("Skinka", 0)]
        [DataRow("Ananas", 0)]
        [DataRow("Kebab", 0)]
        [DataRow("Champinjoner", 0)]
        [DataRow("Lök", 0)]
        [DataRow("Feferoni", 0)]
        [DataRow("Isbergssallad", 0)]
        [DataRow("Tomat", 0)]
        [DataRow("Kebabsås", 0)]
        [DataRow("Räkor", 0)]
        [DataRow("Musslor", 0)]
        [DataRow("Kronärtskocka", 0)]
        [DataRow("Coca cola", 0)]
        [DataRow("Fanta", 0)]
        [DataRow("Sprite", 0)]
        public void Should_contain_default_data_on_creation(string itemName, int itemAmount)
        {
            var repo = new DataStorage();
            var storageItem = repo.StoredItems.Where(x => x.ItemName == itemName).ToList();
            Assert.IsTrue(storageItem.Count(x => x.ItemName == itemName) == 1);
            Assert.AreEqual(storageItem.First().AvailableItemsCount, itemAmount);
        }

        [TestMethod]
        public void Should_change_item_amount()
        {
            var repo = new DataStorage();
            Assert.AreEqual(repo.StoredItems.First().AvailableItemsCount, 0);
            repo.StoredItems.First().AvailableItemsCount = 5;
            Assert.AreEqual(repo.StoredItems.First().AvailableItemsCount, 5);
        }
    }
}
