using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
