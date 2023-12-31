﻿using DAL.Data;
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
        private readonly LiteDatabase _cartDbContext;
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

        public Item? GetItem(int itemId)
        {
            try
            {
                var carts = _cartDbContext.GetCollection<Cart>("Carts").FindAll();
                foreach (var cart in carts)
                {
                    var item = cart.Items.Find(i => i.Id == itemId);
                    if (item != null)
                    {
                        return item;
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
        public int Insert(string? cartId, Item item)
        {
            try
            {
                var carts = _cartDbContext.GetCollection<Cart>("Carts");
                var newId = cartId ?? Guid.NewGuid().ToString();
                var result = false;

                var cart = carts.FindById(newId) ?? new Cart { Id = newId, Items = new List<Item>() };

                if (cart != null)
                {
                    // If cart exists, update the item if it exists in the cart, otherwise, add to cart
                    var existingItem = cart.Items.FirstOrDefault(i => i.Id == item.Id);

                    if (existingItem != null)
                    {
                        existingItem.CartId = item.CartId;
                        existingItem.Name = item.Name;
                        existingItem.Image = item.Image;
                        existingItem.Price = item.Price;
                        existingItem.Quantity = item.Quantity;
                    }
                    else
                    {
                        cart.Items.Add(item);
                    }

                    carts.Upsert(cart);
                }
                else
                {
                    // If cart doesn't exist, create a new cart and add the item
                    cart = new Cart { Id = newId };
                    cart.Items.Add(item);
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
        public IEnumerable<Item> GetItemsFromCart(string cartId)
        {
            var cart = _cartDbContext.GetCollection<Cart>("Carts")
                                .Find(x => x.Id == cartId)
                                .FirstOrDefault();
            var items = new List<Item>();
            if (cart != null)
            {
                items = cart.Items;
            }
            return items;
        }
    }
}
