using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;
using System.Collections.Generic;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {

        public readonly IRepository<Item> _repository;

        public ItemsController(IRepository<Item> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            return (await _repository.GetAllAsync()).AsListDTO();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto itemDto)
        {
            ItemDto item = new(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.price, DateTimeOffset.UtcNow);
            await _repository.CreateAsync(item.AsEntity());
            return CreatedAtAction(nameof(GetAsync), new { id = item.Id }, item);
        }

        [HttpPut]
        public async Task<ActionResult<ItemDto>> Put(Guid Id, UpdateItemDto itemDto)
        {


            var existingItem = (await _repository.GetAsync(Id));

            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.price = itemDto.price;
            await _repository.UpdateAsync(existingItem);
            return NoContent();

        }

        [HttpDelete]
        public async Task<ActionResult<ItemDto>> Delete(Guid id)
        {
            var existingItem = (await _repository.GetAsync(id));

            if (existingItem == null)
            {
                return NotFound();
            }
            await _repository.RemoveAsync(id);
            return NoContent();

        }
    }
}

