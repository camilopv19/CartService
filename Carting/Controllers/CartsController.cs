using BLL;
using DAL.Entities;
using Microsoft.ApplicationInsights;
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
        private readonly ICartService _cartService;
        private readonly TelemetryClient telemetryClient;

#pragma warning disable 1591
        public CartsController(ICartService cartService, TelemetryClient telemetryClient)
        {
            _cartService = cartService;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Gets an Item by its Id
        /// </summary>
        /// <param name="id">The Id of the item to delete.</param>
        /// <returns>Ok</returns>
        [HttpGet("item/{id}")]
        public ActionResult<Item> GetItem(int id)
        {
            var result = _cartService.GetItem(id);
            if (result != null)
            {
                telemetryClient.TrackEvent("Get Item v1");
                return Ok(result);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Get Cart info by Id
        /// </summary>
        /// <param name="cartId">String.</param>
        /// <returns>A cart with its Items, if any</returns>
        [HttpGet("{cartId}", Name = "GetCart")]
        public ActionResult<Cart> Get(string cartId)
        {
            var result = _cartService.GetCart(cartId);
            if (result != default) {
                telemetryClient.TrackEvent("Get Cart v1");
                return Ok(_cartService.GetCart(cartId));
            }
            else
                return NotFound();
        }
        /// <summary>
        /// Get all Carts
        /// </summary>
        /// <returns>A cart with its Items, if any</returns>
        [HttpGet(Name = "GetAll")]
        public ActionResult<Item> GetAll()
        {
            var result = _cartService.GetAll();
            if (result != default){
                telemetryClient.TrackEvent("Get all Items v1");
                return Ok(_cartService.GetAll());
            }
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
            int result = _cartService.Insert(cartId, dto);
            if (result == 0)
            {
                return BadRequest();
            }
            telemetryClient.TrackEvent("Add Item v1");
            return Ok(dto);
        }
        /// <summary>
        /// Deletes an Item by its Id and CartId
        /// </summary>
        /// <param name="cartId">The Id of the Cart containing the Item.</param>
        /// <param name="itemId">The Id of the item to delete.</param>
        /// <returns>Ok</returns>
        [HttpDelete("{cartId}/{itemId}")]
        public ActionResult<Item> Delete(string cartId, int itemId)
        {
            var result = _cartService.Delete(cartId, itemId);
            if (result > 0){
                telemetryClient.TrackEvent("Delete Item v1");
                return Ok($"{result} items were deleted with ItemId = {itemId}");
            }
            else
                return NotFound();
        }
    }
}
