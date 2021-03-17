using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_ShoppingCart_Common
{
    public class ShoppingCartItem
    {
        public int Product_ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public short Quantity { get; set; }

        public override string ToString()
        {
            return $"Product Name: {Name}\n" +
                   $"Price: {Price:c}\n" +
                   $"Quantity: {Quantity}\n" +
                   $"Total amount: {Price*Quantity:c}\n";
        }
    }
}
