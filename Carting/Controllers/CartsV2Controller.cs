using BLL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carts.Controllers
{
    /// <Summary>
    /// Carting API.
    /// </Summary>
    /// 
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiController]
    public class CartsV2Controller : ControllerBase
    {
        private readonly ICartService _itemService;

#pragma warning disable 1591
        public CartsV2Controller(ICartService cartService)
        {
            _itemService = cartService;
        }

        /// <summary>
        /// Get a list of Items.
        /// </summary>
        /// <returns>A list of Items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public IEnumerable<Item> Get() => _itemService.GetItems();


        /// <summary>
        /// Inserts an Item into a Cart, if the cart doesn't exist, it'll be created.
        /// </summary>
        /// <returns>The inserted item</returns>
        [HttpPost]
        public ActionResult<Item> AddItem(string? cartId, Item dto)
        {
#pragma warning disable CS8604 // Possible null reference argument: cartId.
            int result = _itemService.Insert(cartId, dto);
            if (result != default)
                return CreatedAtAction("AddItem", _itemService.GetCart(dto.CartId));
            else
                return BadRequest();
        }
        /// <summary>
        /// Deletes a Item by its Id and CartId
        /// </summary>
        /// <returns>Ok</returns>
        [HttpDelete("{id}")]
        public ActionResult<Item> Delete(string cartId, int itemId)
        {
            var result = _itemService.Delete(cartId, itemId);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }
    }
}
