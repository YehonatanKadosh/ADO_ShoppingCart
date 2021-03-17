using ADO_ShoppingCart_Common;
using ADO_ShoppingCart_Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_ShoppingCart_BLL
{
    public class Customer_Service
    {
        DataBase DB;
        public Customer_Service()
        {
            DB = new DataBase();
        }

        // Get all Products from DB
        public IEnumerable<Product> GetAllProducts()
        => DB.GetAll_Products();

        // get all ShoppingCartItems From DB
        public IEnumerable<ShoppingCartItem> GetAllShoppingCartItems()
        => DB.GetAll_ShoppingCartItems();

        // Updates the Products Table and deleting the ShoppingCart
        public void Update_Products_And_Supply_After_Purchase(IEnumerable<ShoppingCartItem> items)
        => DB.Update_Products_And_Supply_After_Purchase(items);
        
        //calculate price of the items in the Shopping cart
        public double Total_Price(IEnumerable<ShoppingCartItem> items)
        => items.Sum(item => item.Price * item.Quantity);

        //Actions on the Cart Table by queries
        public void Add_Update_Delete_ShoppingCart(CartAction action, Product item, int quantity)
        => DB.Add_Update_Delete_ShoppingCart(action, item, quantity);
        
        // Check weather the product is already in the shopping cart
        public bool Product_Exists_In_ShoppingCart(Product product, IEnumerable<ShoppingCartItem> Items)
        => Items.ToList().Find(item => item.Product_ID == product.Product_ID) != null;

        // get the item from cart based on its ID
        public ShoppingCartItem Get_Item_By_ProductID(int product_ID, IEnumerable<ShoppingCartItem> Items)
        => Items.ToList().Find(item => item.Product_ID == product_ID);

        // check weather the asked amount is legit!
        public bool Can_Supply(Product product, int Asked_Amount)
        => Asked_Amount <= product.Amount_In_Stock;
    }

}
