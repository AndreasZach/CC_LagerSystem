using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public void Removing_many_items_Should_return_no_content_result()
        {
            var storageFake = new DataStorageFake(10);
            var controller = new StorageItemController(new DataRepository(storageFake));
            var result = controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("Ost"){ItemAmount = 1}
            }) as NoContentResult;
            Assert.AreEqual(204, result?.StatusCode);
        }

        [TestMethod]
        public void Removing_many_items_Should_return_not_found_result_If_NotFoundException_is_thrown()
        {
            var storageFake = new DataStorageFake(10);
            var controller = new StorageItemController(new DataRepository(storageFake));
            var result = controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("DOES NOT EXIST"){ItemAmount = 1}
            }) as NotFoundObjectResult;
            Assert.AreEqual(404, result?.StatusCode);
        }

        [TestMethod]
        public void Removing_many_items_Should_return_not_found_result_If_EmptyStorageException_is_thrown()
        {
            var storageFake = new DataStorageFake(10);
            var controller = new StorageItemController(new DataRepository(storageFake));
            var result = controller.RemoveMany(new List<StorageItem>()
            {
                new StorageItem("Ost"){ItemAmount = 11}
            }) as BadRequestObjectResult;
            Assert.AreEqual(400, result?.StatusCode);
        }
    }
}
