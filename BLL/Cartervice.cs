using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Cartervice: ICartService
    {
        private readonly ICartRepository _repository;
        public Cartervice(ICartRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Item> GetItemsFromCart(string cartId) => _repository.GetItemsFromCart(cartId);

        public Cart? GetCart(string cartId) => _repository.GetCart(cartId);
        public int Delete(string cartId, int itemId) => _repository.Delete(cartId, itemId);
        public int Insert(string cartId, Item item) => _repository.Insert(cartId, item);

        public IEnumerable<Cart> GetAll() => _repository.GetAll();
    }
}
