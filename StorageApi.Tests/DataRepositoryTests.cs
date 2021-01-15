using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageApi.Data;
using StorageApi.Exceptions;
using StorageApi.Tests.Fakes;

namespace StorageApi.Tests
{
    [TestClass]
    public class DataRepositoryTests
    {
        [TestMethod]
        public void Getting_item_count_Should_return_count_If_Successful()
        {
            var repo = new DataRepository(new DataStorageFake(1));
            Assert.AreEqual(1, repo.GetItemCount("Ost"));
        }

        [TestMethod]
        public void Getting_item_count_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var repo = new DataRepository(new DataStorageFake());
            Assert.ThrowsException<NotFoundException>(() => 
                repo.GetItemCount("Not an Item"));
        }

        [TestMethod]
        public void Adding_an_item_Should_be_Successful_without_throwing_exceptions()
        {
            var repo = new DataRepository(new DataStorageFake(1));
            repo.AddItemAmount("Ost", 5);
        }

        [TestMethod]
        public void Adding_item_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var repo = new DataRepository(new DataStorageFake());
            Assert.ThrowsException<NotFoundException>(() => 
                repo.AddItemAmount("Not an Item", 5));
        }

        [TestMethod]
        public void Adding_item_Should_throw_FullStorageException_If_total_amount_exceeds_max_capacity()
        {
            var maxCapacity = 500;
            var repo = new DataRepository(new DataStorageFake());
            Assert.ThrowsException<FullStorageException>(() => 
                repo.AddItemAmount("Ost", maxCapacity + 1));
        }

        [TestMethod]
        public void Removing_an_item_Should_be_Successful_without_throwing_exceptions()
        {
            var repo = new DataRepository(new DataStorageFake(1));
            repo.RemoveItemAmount("Ost", 1);
        }

        [TestMethod]
        public void Removing_item_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var repo = new DataRepository(new DataStorageFake());
            Assert.ThrowsException<NotFoundException>(() => 
                repo.RemoveItemAmount("Not an Item", 5));
        }

        [TestMethod]
        public void Removing_item_Should_throw_EmptyStorageException_If_total_amount_becomes_less_than_zero()
        {
            var repo = new DataRepository(new DataStorageFake());
            Assert.ThrowsException<EmptyStorageException>(() => 
                repo.RemoveItemAmount("Ost", 1));
        }

        [TestMethod]
        public void Add_items_to_all_Should_succeed_without_throwing_exceptions()
        {
            var repo = new DataRepository(new DataStorageFake());
            repo.AddItemAmountToAll();
        }

        [TestMethod]
        public void Add_items_to_all_Should_throw_FullStorageException_If_any_item_exceeds_storage_capacity()
        {
            var repo = new DataRepository(new DataStorageFake(499));
            Assert.ThrowsException<FullStorageException>(() => repo.AddItemAmountToAll());
        }

    }
}
