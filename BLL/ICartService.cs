using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ICartService
    {
        IEnumerable<Item> GetItems();
        Cart? GetCart(string cartId);
        int Delete(string cartId, int itemId);
        int Insert(string cartId, Item item);

        public IEnumerable<Cart> GetAll();
    }
}
