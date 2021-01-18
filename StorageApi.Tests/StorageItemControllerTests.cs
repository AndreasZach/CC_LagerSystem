using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageApi.Controllers;
using StorageApi.Data;
using StorageApi.Models;
using StorageApi.Tests.Fakes;

namespace StorageApi.Tests
{
    [TestClass]
    public class StorageItemControllerTests
    {

        [TestMethod]
        public async Task Removing_many_items_Should_return_no_content_result()
        {
            var contextWithData = CreateContextWithData(10);
            var controller = new StorageItemController(new DataRepository(contextWithData));
            var result = await controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("Ost"){ItemAmount = 1}
            }) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_many_items_Should_return_not_found_result_If_NotFoundException_is_thrown()
        {
            var contextWithData = CreateContextWithData(10);
            var controller = new StorageItemController(new DataRepository(contextWithData));
            var result = await controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("DOES NOT EXIST"){ItemAmount = 1}
            }) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_many_items_Should_return_not_found_result_If_EmptyStorageException_is_thrown()
        {
            var contextWithData = CreateContextWithData(10);
            var controller = new StorageItemController(new DataRepository(contextWithData));
            var result = await controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("Ost"){ItemAmount = 11}
            }) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
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
