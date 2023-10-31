using DAL.Data;
using DAL.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public class CartRepository : ICartRepository
    {
        private LiteDatabase _cartDbContext;
        public CartRepository(ILiteDbContext cartDbContext)
        {
            _cartDbContext = cartDbContext.Database;
        }
        public Cart? GetCart(string cartId)
        {
            return _cartDbContext.GetCollection<Cart>("Carts")
                                 .Find(x => x.Id == cartId)
                                 .FirstOrDefault();
        }
        public IEnumerable<Cart> GetAll()
        {
            return _cartDbContext.GetCollection<Cart>("Carts").FindAll();
        }

        public int Insert(string? cartId, Item item)
        {
            try
            {
                var carts = _cartDbContext.GetCollection<Cart>("Carts");
                var newId = cartId == null ? Guid.NewGuid().ToString() : cartId;
                var result = false;

                var cart = carts.FindById(newId) ?? new Cart { Id = newId, Items = new List<Item>() };

                if (cart != null)
                {
                    // Add the item to the cart
                    cart.Items.Add(item);

                    // Upsert the cart into the collection
                    result = carts.Upsert(cart);

                }
                return 1; //When the cart exists it gets updated on its new item(s) and False is returned
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(string cartId, int itemId)
        {
            try
            {
                Cart? cart = GetCart(cartId);
                if (cart != null)
                {
                    var deletedCount = cart.Items.RemoveAll(x => x.Id == itemId);
                    _cartDbContext.GetCollection<Cart>("Carts").Update(cart);
                    return deletedCount;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        //V2 method
        public IEnumerable<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            var carts = _cartDbContext.GetCollection<Cart>("Carts").FindAll();
            foreach (var cart in carts)
            {
                if (cart.Items.Any())
                {
                    items.AddRange(cart.Items);
                }
            }
            return items;
        }
    }
}
