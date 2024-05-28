using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace Bookstore_App
{
    public partial class showCart : Window
    {
        public ObservableCollection<CartItem> CartItems { get; set; }
        private int customerId;

        public showCart(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            CartItems = new ObservableCollection<CartItem>();
            ItemList.ItemsSource = CartItems;
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT b.title FROM cart c JOIN books b ON c.bookID = b.bookID WHERE c.customerID = @customerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@customerID", customerId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CartItems.Add(new CartItem
                        {
                            Title = reader.GetString(0).Trim()
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cart items: " + ex.Message);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.SelectedItem is CartItem selectedItem)
            {
                string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
                string query = "DELETE FROM cart WHERE customerID = @customerID AND bookID = (SELECT bookID FROM books WHERE title = @title)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@customerID", customerId);
                        command.Parameters.AddWithValue("@title", selectedItem.Title);

                        command.ExecuteNonQuery();
                    }
                    CartItems.Remove(selectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing item: " + ex.Message);
                }
            }
        }
        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checkout functionality not implemented.");
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "DELETE FROM cart WHERE customerID = @customerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@customerID", customerId);

                    command.ExecuteNonQuery();
                }
                CartItems.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing cart: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BookCartDetails bookCartDetails = new BookCartDetails(customerId);
            bookCartDetails.Show();
            this.Close();
        }

        private void ItemList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            removeItemButton.IsEnabled = ItemList.SelectedItem != null;
        }
    }

    public class CartItem
    {
        public string Title { get; set; }
    }
}