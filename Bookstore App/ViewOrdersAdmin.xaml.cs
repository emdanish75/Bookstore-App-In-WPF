using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore_App
{
    public partial class ViewOrdersAdmin : Window
    {
        public ViewOrdersAdmin()
        {
            InitializeComponent();
            LoadOrders();
            approveButton.IsEnabled = false;
            disapproveButton.IsEnabled = false;
            paymentButton.IsEnabled = false;
            ordersDataGrid.SelectionChanged += OrdersDataGrid_SelectionChanged;
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isSelected = ordersDataGrid.SelectedItem != null;
            approveButton.IsEnabled = isSelected;
            disapproveButton.IsEnabled = isSelected;
            paymentButton.IsEnabled = isSelected;
        }

        private void LoadOrders()
        {
            List<OrderDetails> orders = new List<OrderDetails>();

            string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT orderID, titles, totalBill, numberOfBooks, paymentImagePath, orderStatus FROM orders";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderDetails order = new OrderDetails
                        {
                            OrderID = reader.GetInt32(0),
                            Titles = reader.GetString(1),
                            TotalBill = reader.GetInt32(2),
                            NumberOfBooks = reader.GetInt32(3),
                            PaymentImagePath = reader.GetString(4),
                            OrderStatus = reader.GetString(5)
                        };
                        orders.Add(order);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }

            ordersDataGrid.ItemsSource = orders;
        }

        private void UpdateOrderStatus(string status)
        {
            if (ordersDataGrid.SelectedItem is OrderDetails selectedOrder)
            {
                string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
                string query = "UPDATE orders SET orderStatus = @status WHERE orderID = @orderID";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@orderID", selectedOrder.OrderID);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating order status: " + ex.Message);
                }

                LoadOrders();
                ordersDataGrid.SelectedItem = null;
                approveButton.IsEnabled = false;
                disapproveButton.IsEnabled = false;
                paymentButton.IsEnabled = false;
            }
        }

        private void approveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to approve this order?", "Confirm Approval", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                UpdateOrderStatus("Approved");
            }
        }

        private void disapproveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to disapprove this order?", "Confirm Disapproval", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                UpdateOrderStatus("Disapproved");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            this.Close();
            adminMenu.Show();
        }
    }
}
