using BLL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carts.Controllers
{
    /// <Summary>
    /// Carting API.
    /// </Summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _itemService;

#pragma warning disable 1591
        public CartsController(ICartService cartService)
        {
            _itemService = cartService;
        }

        /// <summary>
        /// Get Cart info by Id
        /// </summary>
        /// <param name="cartId">String.</param>
        /// <returns>A cart with its Items, if any</returns>
        [HttpGet("{cartId}", Name = "GetCart")]
        public ActionResult<Item> Get(string cartId)
        {
            var result = _itemService.GetCart(cartId);
            if (result != default)
                return Ok(_itemService.GetCart(cartId));
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts an Item into a Cart, if the cart doesn't exist, it'll be created.
        /// </summary>
        /// <returns>The inserted item</returns>
        [HttpPost]
        public ActionResult<Item> AddItem(string? cartId, Item dto)
        {
#pragma warning disable CS8604 // Possible null reference argument: cartId.
            int result = _itemService.Insert(cartId, dto);
            return CreatedAtAction("AddItem", _itemService.GetCart(dto.CartId));
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
