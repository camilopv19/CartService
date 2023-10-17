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

        public IEnumerable<Item> GetItems()
        {
            return _cartDbContext.GetCollection<Item>("Cart").FindAll(); ;
        }
        public Item? FindOne(int id)
        {
            return _cartDbContext.GetCollection<Item>("Cart")
                                 .Find(x => x.Id == id)
                                 .FirstOrDefault();
        }

        public int Insert(Item cart)
        {
            return _cartDbContext.GetCollection<Item>("Cart").Insert(cart);
        }

        public bool Update(Item cart)
        {
            return _cartDbContext.GetCollection<Item>("Cart")
                .Update(cart);
        }

        public int Delete(int id)
        {
            return _cartDbContext.GetCollection<Item>("Cart")
                .DeleteMany(x => x.Id == id);
        }
    }
}
