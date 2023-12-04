using DAL.Entities;
using DAL.Interfaces;

namespace BLL
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Item> GetItemsFromCart(string cartId) => _repository.GetItemsFromCart(cartId);

        public Cart? GetCart(string cartId) => _repository.GetCart(cartId);
        public Item? GetItem(int itemId) => _repository.GetItem(itemId);
        public int Delete(string cartId, int itemId) => _repository.Delete(cartId, itemId);
        public int Insert(string cartId, Item item) => _repository.Insert(cartId, item);

        public IEnumerable<Cart> GetAll() => _repository.GetAll();
    }
}
