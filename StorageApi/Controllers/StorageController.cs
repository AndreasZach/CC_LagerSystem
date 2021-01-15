using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Exceptions;
using StorageApi.Interfaces;

namespace StorageApi.Controllers
{
    public class StorageController : Controller
    {
        private IDataRepository _repository;

        public StorageController(IDataRepository repository)
        {
            _repository = repository;
        }

        public IActionResult GetItemCount(string itemName)
        {
            try
            {
                return Ok(_repository.GetItemCount(itemName));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut]
        public IActionResult AddItemAmount(string itemName, int amountToAdd)
        {
            try
            {
                _repository.AddItemAmount(itemName, amountToAdd);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (FullStorageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult RemoveItemAmount(string itemName, int amountToRemove)
        {
            try
            {
                _repository.RemoveItemAmount(itemName, amountToRemove);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (EmptyStorageException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
