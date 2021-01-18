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
    public class StorageControllerTests
    {
        [TestMethod]
        public async Task Getting_item_count_Should_return_Ok_result_with_count_If_item_exists()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.GetItemCount("Ost") as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(1, result?.Value);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Getting_item_count_Should_return_not_found_result_If_item_does_not_exist()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.GetItemCount("Does Not Exist") as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_an_item_Should_return_no_content_result_If_successful()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.AddItemAmount("Ost", 5) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_an_item_Should_return_not_found_result_If_no_matching_item_exist()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.AddItemAmount("Does not exist", 5) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_an_item_Should_return_bad_request_result_If_adding_more_items_than_fits_in_storage()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.AddItemAmount("Ost", 501) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_an_item_Should_return_no_content_result_If_successful()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.RemoveItemAmount("Ost", 1) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_an_item_Should_return_not_found_result_If_no_matching_item_exist()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.RemoveItemAmount("Does not exist", 5) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Removing_an_item_Should_return_bad_request_result_If_removing_more_items_than_is_available()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.RemoveItemAmount("Ost", 5) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_items_to_all_Should_return_no_content_result_If_successful()
        {
            var contextWithData = CreateContextWithData(1);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.AddItemAmountToAll() as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Adding_items_to_all_Should_return_bad_request_result_If_any_item_exceeds_max_capacity()
        {
            var contextWithData = CreateContextWithData(499);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.AddItemAmountToAll() as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
            await contextWithData.Database.EnsureDeletedAsync();
        }

        [TestMethod]
        public async Task Getting_all_items_Should_return_all_items()
        {
            var contextWithData = CreateContextWithData(10);
            var controller = new StorageController(new DataRepository(contextWithData));
            var result = await controller.Index() as ViewResult;
            Assert.AreEqual(contextWithData.StorageItems.Count(), (result?.Model as IEnumerable<StorageItem>).Count());
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
