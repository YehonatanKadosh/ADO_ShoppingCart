using ADO_ShoppingCart_Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ADO_ShoppingCart_Dal
{
    public class DataBase
    {
        string _connection_string;

        public DataBase()
        {
            _connection_string = "Data Source=YEHONATAN;Initial Catalog=ADO_Yehonatan_DB;Integrated Security=True";
        }

        //Read the Product Table and return IEnumerable of Products !except of those who ran out!
        public IEnumerable<Product> GetAll_Products()
        {
            DataTable Products = new DataTable();
            string selection_Query = "Select * from Products";
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                using (SqlCommand command = new SqlCommand(selection_Query, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(Products);
                }
            }
            foreach (DataRow row in Products.Rows)
            {
                if (Convert.ToInt32(row["Amount_In_Stock"]) != 0)
                    yield return new Product
                    {
                        Product_ID = Convert.ToInt32(row["Product_ID"]),
                        Product_Name = (row["Product_Name"]).ToString(),
                        Product_Price = float.Parse(row["Product_Price"].ToString()),
                        Amount_In_Stock = Convert.ToInt32(row["Amount_In_Stock"])
                    };
            }
        }

        //Read the ShoppingCartItem Table and return IEnumerable of ShoppingCartItems
        public IEnumerable<ShoppingCartItem> GetAll_ShoppingCartItems()
        {
            DataTable ShoppingCartItems = new DataTable();
            string selection_Query = "select p.Product_ID, Product_Name 'Name', Product_Price 'Price', Quantity " +
                                     "from ShoppingCartItems sc, Products p " +
                                     "where p.Product_ID = sc.Product_ID";
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                using (SqlCommand command = new SqlCommand(selection_Query, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(ShoppingCartItems);
                }
            }
            foreach (DataRow row in ShoppingCartItems.Rows)
            {
                yield return new ShoppingCartItem
                {
                    Product_ID = Convert.ToInt32(row["Product_ID"]),
                    Name = (row["Name"]).ToString(),
                    Price = float.Parse(row["Price"].ToString()),
                    Quantity = Convert.ToInt16(row["Quantity"])
                };
            }
        }

        // Updates the Products Table and deleting the ShoppingCart
        public void Update_Products_And_Supply_After_Purchase(IEnumerable<ShoppingCartItem> items)
        {
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                connection.Open();
                foreach (ShoppingCartItem item in items)
                {
                    string Query =//update the products quantity
                                  $"UPDATE Products " +
                                  $"SET Amount_In_Stock = Amount_In_Stock - {item.Quantity} " +
                                  $"WHERE Product_ID = {item.Product_ID} ;" +
                                  // Delete the item from the Shopping Cart
                                  $"DELETE FROM ShoppingCartItems " +
                                  $"WHERE Product_ID = {item.Product_ID}";
                    // execute query
                    using (SqlCommand command = new SqlCommand(Query, connection)) { command.ExecuteNonQuery(); }
                }
            }
        }

        //Actions on the Cart Table by queries
        public void Add_Update_Delete_ShoppingCart(CartAction action, Product item, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                connection.Open();
                string Query = "";
                switch (action)
                {
                    case CartAction.Update:
                        Query = "UPDATE ShoppingCartItems " +
                               $"SET Quantity = {quantity}" +
                               $"WHERE Product_ID = {item.Product_ID}";
                        break;
                    case CartAction.Add:
                        Query = "INSERT INTO ShoppingCartItems " +
                               $"VALUES({item.Product_ID}, {quantity});";
                        break;
                    case CartAction.Delete:
                        Query = "Delete from ShoppingCartItems " +
                               $"Where Product_ID = {item.Product_ID}";
                        break;
                }
                using (SqlCommand command = new SqlCommand(Query, connection)) { command.ExecuteNonQuery(); }

            }
        }

    }

}
