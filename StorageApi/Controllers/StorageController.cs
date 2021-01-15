using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Interfaces;

namespace StorageApi.Controllers
{
    public class StorageController : Controller
    {
        private IDataStorage _repository;

        public StorageController(IDataStorage repository)
        {
            _repository = repository;
        }

        public IActionResult GetItemCount(string itemName)
        {
            var items = _repository.StoredItems.Where(x => x.ItemName == itemName).ToList();
            if (!items.Any())
                return NotFound();
            return Ok(items.Count());
        }

        [HttpPut]
        public IActionResult AddItemAmount(string itemName, int amountToAdd)
        {
            var itemToUpdate = _repository.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
            if (itemToUpdate is null)
                return NotFound();
            _repository.StoredItems[_repository.StoredItems.IndexOf(itemToUpdate)].AvailableItemsCount += amountToAdd;
            return NoContent();
        }

        [HttpPut]
        public IActionResult RemoveItemAmount(string itemName, int amountToRemove)
        {
            var itemToUpdate = _repository.StoredItems.FirstOrDefault(x => x.ItemName == itemName);
            if (itemToUpdate is null)
                return NotFound();
            var itemIndex = _repository.StoredItems.IndexOf(itemToUpdate);
            if (itemToUpdate.AvailableItemsCount - amountToRemove < 0)
                return BadRequest();
            else
                _repository.StoredItems[itemIndex].AvailableItemsCount -= amountToRemove;
            return NoContent();
        }
    }
}
