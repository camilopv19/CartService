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
    public class CartService: ICartService
    {
        private readonly ICartRepository _repository;
        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Item> GetItems() => _repository.GetItems();

        public Item? FindOne(int id) => _repository.FindOne(id);
        public int Delete(int id) => _repository.Delete(id);
        public int Insert(Item cart) => _repository.Insert(cart);
        public bool Update(Item cart) => _repository.Update(cart);
    }
}
