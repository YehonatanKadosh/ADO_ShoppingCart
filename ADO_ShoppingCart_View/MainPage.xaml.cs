using ADO_ShoppingCart_BLL;
using ADO_ShoppingCart_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ADO_ShoppingCart_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Customer_Service customer_service;
        public MainPage()
        {
            this.InitializeComponent();
            customer_service = new Customer_Service();
            Products.ItemsSource = customer_service.GetAllProducts();
            Shopping_Cart.ItemsSource = customer_service.GetAllShoppingCartItems();
            ShoppingCart_Block.Text = $"Cart {customer_service.Total_Price(Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>):c}";

        }
        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            // if there's no items ther's nothing to buy
            if (Shopping_Cart.Items.Count != 0)
            {
                //update the cart to nothing and the products to remove the quantity bought
                customer_service.Update_Products_And_Supply_After_Purchase(Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>);
                //refresh the listviews
                Products.ItemsSource = customer_service.GetAllProducts();
                Shopping_Cart.ItemsSource = customer_service.GetAllShoppingCartItems();
                ShoppingCart_Block.Text = $"Cart {customer_service.Total_Price(Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>):c}";
            }
        }

        private void Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Products.SelectedItem != null)
            {
                // we want to show the quantity meter
                Product_Update_In_Cart.Visibility = Product_Minus.Visibility = Product_Plus.Visibility = Visibility.Visible;
                // get the info from the cart if the product already there and 0 otherwise
                if (customer_service.Product_Exists_In_ShoppingCart(Products.SelectedItem as Product, Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>))
                    Product_Update_In_Cart.Text = customer_service.Get_Item_By_ProductID((Products.SelectedItem as Product).Product_ID, Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>).Quantity.ToString();
                else
                    Product_Update_In_Cart.Text = "0";
            }
            else
                // when we refresh the list the selection changes to null
                Product_Update_In_Cart.Visibility = Product_Minus.Visibility = Product_Plus.Visibility = Visibility.Collapsed;
        }

        // add 1 to cart of the selected product if possible and update database or add to cart
        private void Product_Plus_Click(object sender, RoutedEventArgs e)
        {
            if (customer_service.Can_Supply(Products.SelectedItem as Product, int.Parse(Product_Update_In_Cart.Text) + 1))
            {
                Product_Update_In_Cart.Text = (int.Parse(Product_Update_In_Cart.Text) + 1).ToString();
                if (customer_service.Product_Exists_In_ShoppingCart(Products.SelectedItem as Product, Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>))
                    customer_service.Add_Update_Delete_ShoppingCart(CartAction.Update, Products.SelectedItem as Product, int.Parse(Product_Update_In_Cart.Text));
                else
                    customer_service.Add_Update_Delete_ShoppingCart(CartAction.Add, Products.SelectedItem as Product, int.Parse(Product_Update_In_Cart.Text));
                // Update listview
                Shopping_Cart.ItemsSource = customer_service.GetAllShoppingCartItems();
                ShoppingCart_Block.Text = $"Cart {customer_service.Total_Price(Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>):c}";

            }
        }

        // remove 1 of selected product from cart if possible and update database or delete if 0
        private void Product_Minus_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(Product_Update_In_Cart.Text) - 1 > 0)
            {
                Product_Update_In_Cart.Text = (int.Parse(Product_Update_In_Cart.Text) - 1).ToString();
                customer_service.Add_Update_Delete_ShoppingCart(CartAction.Update, Products.SelectedItem as Product, int.Parse(Product_Update_In_Cart.Text));
            }
            else
            {
                customer_service.Add_Update_Delete_ShoppingCart(CartAction.Delete, Products.SelectedItem as Product, 0);
                Product_Update_In_Cart.Text = "0";
            }
            // Update listview
            Shopping_Cart.ItemsSource = customer_service.GetAllShoppingCartItems();
            ShoppingCart_Block.Text = $"Cart {customer_service.Total_Price(Shopping_Cart.ItemsSource as IEnumerable<ShoppingCartItem>):C}";

        }
    }
}
