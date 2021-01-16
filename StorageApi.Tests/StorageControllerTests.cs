using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
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
        public void Getting_item_count_Should_return_Ok_result_with_count_If_item_exists()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.GetItemCount("Ost") as OkObjectResult;
            Assert.AreEqual(200, result?.StatusCode);
            Assert.AreEqual(1, result?.Value);
        }

        [TestMethod]
        public void Getting_item_count_Should_return_not_found_result_If_item_does_not_exist()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.GetItemCount("Does Not Exist") as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
        }

        [TestMethod]
        public void Adding_an_item_Should_return_no_content_result_If_successful()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.AddItemAmount("Ost", 5) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
        }

        [TestMethod]
        public void Adding_an_item_Should_return_not_found_result_If_no_matching_item_exist()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.AddItemAmount("Does not exist", 5) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
        }

        [TestMethod]
        public void Adding_an_item_Should_return_bad_request_result_If_adding_more_items_than_fits_in_storage()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.AddItemAmount("Ost", 501) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public void Removing_an_item_Should_return_no_content_result_If_successful()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.RemoveItemAmount("Ost", 1) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
        }

        [TestMethod]
        public void Removing_an_item_Should_return_not_found_result_If_no_matching_item_exist()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.RemoveItemAmount("Does not exist", 5) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
        }

        [TestMethod]
        public void Removing_an_item_Should_return_bad_request_result_If_removing_more_items_than_is_available()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.RemoveItemAmount("Ost", 5) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public void Adding_items_to_all_Should_return_no_content_result_If_successful()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(1)));
            var result = controller.AddItemAmountToAll() as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
        }

        [TestMethod]
        public void Adding_items_to_all_Should_return_bad_request_result_If_any_item_exceeds_max_capacity()
        {
            var controller = new StorageController(new DataRepository(new DataStorageFake(499)));
            var result = controller.AddItemAmountToAll() as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public void Getting_all_items_Should_return_all_items()
        {
            var storageFake = new DataStorageFake(10);
            var controller = new StorageController(new DataRepository(storageFake));
            var result = controller.Index() as ViewResult;
            Assert.AreEqual(storageFake.StoredItems, result?.Model);
        }
    }
}
