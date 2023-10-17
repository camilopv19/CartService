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
        Item? FindOne(int id);
        int Delete(int id);
        int Insert(Item cart);
        bool Update(Item cart);

    }
}
