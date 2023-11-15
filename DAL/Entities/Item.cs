using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Item
    {
        public int Id { get; set; } = 0;
        public string CartId { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Item(MessageItem messageItem)
        {
            // Catalog item signature Mapping
            Id = messageItem.Id;
            Name = messageItem.Name;
            Image = messageItem.Image;
            Price = Convert.ToDouble(messageItem.Price);
            Quantity = messageItem.Amount;
        }

        public Item()
        {
            
        }
    }
}
