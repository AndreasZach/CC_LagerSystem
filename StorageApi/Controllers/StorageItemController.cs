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
    
    [ApiController]
    [Route("api/[controller]")]
    public class StorageItemController : ControllerBase
    {
        private readonly IDataRepository _repository;

        public StorageItemController(IDataRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult RemoveMany(List<StorageItem> items)
        {
            try
            {
                _repository.RemoveMany(items);
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
