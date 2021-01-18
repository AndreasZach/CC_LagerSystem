using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageApi.Data;
using StorageApi.Exceptions;
using StorageApi.Models;
using StorageApi.Tests.Fakes;

namespace StorageApi.Tests
{
    [TestClass]
    public class DataRepositoryTests
    {

        [TestMethod]
        public async Task Getting_item_count_Should_return_count_If_Successful()
        {
            var contextWithData = CreateContextWithData(1);
            var repo = new DataRepository(contextWithData);
            var itemCount = await repo.GetItemCountAsync("Ost");
            Assert.AreEqual(1, itemCount);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Getting_item_count_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => 
                repo.GetItemCountAsync("Not an Item"));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_an_item_Should_be_Successful_without_throwing_exceptions()
        {
            var contextWithData = CreateContextWithData(1);
            var repo = new DataRepository(contextWithData);
            await repo.AddItemAmountAsync("Ost", 5);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_item_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => 
                repo.AddItemAmountAsync("Not an Item", 5));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_item_Should_throw_FullStorageException_If_total_amount_exceeds_max_capacity()
        {
            var maxCapacity = 500;
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<FullStorageException>(() => 
                repo.AddItemAmountAsync("Ost", maxCapacity + 1));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_an_item_Should_be_Successful_without_throwing_exceptions()
        {
            var contextWithData = CreateContextWithData(1);
            var repo = new DataRepository(contextWithData);
            await repo.RemoveItemAmountAsync("Ost", 1);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_item_Should_throw_NotFoundException_If_no_matching_item_exists()
        {
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => 
                repo.RemoveItemAmountAsync("Not an Item", 5));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_item_Should_throw_EmptyStorageException_If_total_amount_becomes_less_than_zero()
        {
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<EmptyStorageException>(() => 
                repo.RemoveItemAmountAsync("Ost", 1));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Add_items_to_all_Should_succeed_without_throwing_exceptions()
        {
            var contextWithData = CreateContextWithData(0);
            var repo = new DataRepository(contextWithData);
            await repo.AddItemAmountToAllAsync();
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Add_items_to_all_Should_throw_FullStorageException_If_any_item_exceeds_storage_capacity()
        {
            var contextWithData = CreateContextWithData(499);
            var repo = new DataRepository(contextWithData);
            await Assert.ThrowsExceptionAsync<FullStorageException>(() => repo.AddItemAmountToAllAsync());
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Getting_all_storage_items_Should_return_collection_of_items()
        {
            var contextWithData = CreateContextWithData(10);
            var repo = new DataRepository(contextWithData);
            var resultData = await repo.GetAllItemsAsync();
            Assert.AreEqual(contextWithData.StorageItems.Count(), resultData.Count());
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Remove_all_item_amounts_specified_in_collection_should_succeed_without_throwing_exceptions()
        {
            var contextWithData = CreateContextWithData(10);
            var repo = new DataRepository(contextWithData);
            var items = new List<StorageItem>()
            {
                new StorageItem("Ost") {ItemAmount = 3},
                new StorageItem("Tomat") {ItemAmount = 5}
            };
            await repo.RemoveManyAsync(items);
            Assert.AreEqual(7, contextWithData.StorageItems.First(
                x => x.ItemName == "Ost").ItemAmount);
            Assert.AreEqual(5, contextWithData.StorageItems.First(
                x => x.ItemName == "Tomat").ItemAmount);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Remove_all_item_amounts_specified_in_collection_should_throw_NotFoundException_if_an_item_cannot_be_found()
        {
            var contextWithData = CreateContextWithData(10);
            var repo = new DataRepository(contextWithData);
            var items = new List<StorageItem>()
            {
                new StorageItem("Ost") {ItemAmount = 3},
                new StorageItem("DOES NOT EXIST") {ItemAmount = 5}
            };

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => repo.RemoveManyAsync(items));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Remove_all_item_amounts_specified_in_collection_should_throw_EmptyStorageException_if_amount_less_than_zero()
        {
            var contextWithData = CreateContextWithData(10);
            var repo = new DataRepository(contextWithData);
            var items = new List<StorageItem>()
            {
                new StorageItem("Ost") {ItemAmount = 3},
                new StorageItem("Tomat") {ItemAmount = 11}
            };

            await Assert.ThrowsExceptionAsync<EmptyStorageException>(() => repo.RemoveManyAsync(items));
            await contextWithData.Database.EnsureDeletedAsync();
        }

        private StorageItemContext CreateContextWithData(int itemAmountPerItem) {
            var testData = new TestData().GetDefaultTestData(itemAmountPerItem);
            var options = new DbContextOptionsBuilder<StorageItemContext>()
                .UseInMemoryDatabase(databaseName: "MockStorageItemDatabase")
                .Options;
            var storageItemContext = new StorageItemContext(options);
            storageItemContext.StorageItems.AddRange(testData);
            storageItemContext.SaveChanges();
            return storageItemContext;
        }
    }
}
