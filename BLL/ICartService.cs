using DAL.Entities;

namespace BLL
{
    public interface ICartService
    {
        int Delete(string cartId, int itemId);
        IEnumerable<Cart> GetAll();
        Cart? GetCart(string cartId);
        Item? GetItem(int itemId);
        IEnumerable<Item> GetItemsFromCart(string cartId);
        int Insert(string cartId, Item item);
    }
}