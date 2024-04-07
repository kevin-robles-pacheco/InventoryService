using InventoryService.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryServiceHandler  _inventoryService; 
        public InventoryController(InventoryServiceHandler inventoryServiceHandler) 
        {
            _inventoryService = inventoryServiceHandler;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _inventoryService.GetInventory();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se puedo obtener la información solicitada. {ex.Message}");
            }            
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var response = await _inventoryService.GetInventory("id", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se puedo obtener la información solicitada. {ex.Message}");
            }
        }

        // POST api/<InventoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
