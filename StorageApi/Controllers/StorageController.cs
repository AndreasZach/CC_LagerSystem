using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorageApi.Exceptions;
using StorageApi.Interfaces;
using StorageApi.Models;

namespace StorageApi.Controllers
{
    public class StorageController : Controller
    {
        private IDataRepository _repository;

        public StorageController(IDataRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult GetItemCount(string id)
        {
            try
            {
                return Ok(_repository.GetItemCount(id));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddItemAmount(string id, int amountToAdd)
        {
            try
            {
                _repository.AddItemAmount(id, amountToAdd);
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

        [HttpPost]
        public IActionResult RemoveItemAmount(string id, int amountToRemove)
        {
            try
            {
                _repository.RemoveItemAmount(id, amountToRemove);
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

        [HttpPost]
        public IActionResult AddItemAmountToAll()
        {
            try
            {
                _repository.AddItemAmountToAll();
                return NoContent();
            }
            catch (FullStorageException e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult Index()
        {
            return View(_repository.GetAllItems());
        }
    }
}
