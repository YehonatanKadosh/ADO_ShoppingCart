using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_ShoppingCart_Common
{
    public class Product
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public float Product_Price { get; set; }
        public int Amount_In_Stock { get; set; }

        public override string ToString()
        {
            return $"Name: {Product_Name}\n" +
                   $"Price: {Product_Price:c}\n" +
                   $"Amount In Stock: {Amount_In_Stock}\n";
        }
    }
}
