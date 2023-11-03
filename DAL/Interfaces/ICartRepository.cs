using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICartRepository
    {
        IEnumerable<Item> GetItemsFromCart(string cartId);
        Cart? GetCart(string cartId);
        int Delete(string cartId, int itemId);
        int Insert(string? cartId, Item item);
        public IEnumerable<Cart> GetAll();
    }
}
