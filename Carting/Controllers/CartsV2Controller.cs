﻿using BLL;
using DAL.Entities;
using Microsoft.ApplicationInsights;
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
    public class Carts2Controller : ControllerBase
    {
        private readonly ICartService _itemService;
        private readonly TelemetryClient telemetryClient;

#pragma warning disable 1591
        public Carts2Controller(ICartService cartService, TelemetryClient telemetryClient)
        {
            _itemService = cartService;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Get Cart info v2: Only the list of Items.
        /// </summary>
        /// <param name="cartId">String.</param>
        /// <returns>A list of Items.</returns>
        [HttpGet("{cartId}", Name = "GetCart")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public IEnumerable<Item> Get(string cartId)
        {
            telemetryClient.TrackEvent("Get Cart v2");
            return _itemService.GetItemsFromCart(cartId);
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
            if (result == 0)
            {
                return BadRequest();
            }
            telemetryClient.TrackEvent("Add item v2");
            return Ok(dto);
        }
        /// <summary>
        /// Deletes a Item by its Id and CartId
        /// </summary>
        /// <param name="cartId">The Id of the Cart containing the Item.</param>
        /// <param name="itemId">The Id of the item to delete.</param>
        /// <returns>Ok</returns>
        [HttpDelete("{cartId}/{itemId}")]
        public ActionResult<Item> Delete(string cartId, int itemId)
        {
            var result = _itemService.Delete(cartId, itemId);
            if (result > 0)
            {
                telemetryClient.TrackEvent("Deleted Item v2");
                return Ok($"{result} items were deleted with ItemId = {itemId}");
            }
            else
                return NotFound();
        }
    }
}
