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
        public async Task<IActionResult> GetItemCount(string id)
        {
            try
            {
                return Ok(await _repository.GetItemCountAsync(id));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddItemAmount(string id, int amountToAdd)
        {
            try
            {
                await _repository.AddItemAmountAsync(id, amountToAdd);
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
        public async Task<IActionResult> RemoveItemAmount(string id, int amountToRemove)
        {
            try
            {
                await _repository.RemoveItemAmountAsync(id, amountToRemove);
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
        public async Task<IActionResult> AddItemAmountToAll()
        {
            try
            {
                await _repository.AddItemAmountToAllAsync();
                return NoContent();
            }
            catch (FullStorageException e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllItemsAsync());
        }
    }
}
