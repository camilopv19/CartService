using BLL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get a list of Items.
        /// </summary>
        /// <returns>A list of Items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public IEnumerable<Item> Get() => _cartService.GetItems();

        /// <summary>
        /// Finds an Item by its Id
        /// </summary>
        /// <param name="Id">Integer.</param>
        /// <returns>An Items</returns>
        [HttpGet("{id}", Name = "FindOne")]
        public ActionResult<Item> Get(int id)
        {
            var result = _cartService.FindOne(id);
            if (result != default)
                return Ok(_cartService.FindOne(id));
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts an Item
        /// </summary>
        /// <returns>The inserted item</returns>
        [HttpPost]
        public ActionResult<Item> Insert(Item dto)
        {
            var id = _cartService.Insert(dto);
            if (id != default)
                return CreatedAtAction("Insert", _cartService.FindOne(id));
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates an Item
        /// </summary>
        /// <returns>1</returns>
        [HttpPut]
        public ActionResult<Item> Update(Item dto)
        {
            var result = _cartService.Update(dto);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes an Item by its Id
        /// </summary>
        /// <returns>1</returns>
        [HttpDelete("{id}")]
        public ActionResult<Item> Delete(int id)
        {
            var result = _cartService.Delete(id);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }
    }
}
