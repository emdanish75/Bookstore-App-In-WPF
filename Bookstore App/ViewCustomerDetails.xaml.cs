using System;
using System.Collections.Generic;
using System.Data.SqlClient; // Add this namespace for SQL Server access
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore_App
{
    /// <summary>
    /// Interaction logic for ViewCustomerDetails.xaml
    /// </summary>
    public partial class ViewCustomerDetails : Window
    {
        public ViewCustomerDetails()
        {
            InitializeComponent();
            LoadCustomerData(); // Load data when the window is initialized
        }

        private void LoadCustomerData()
        {
            // Replace with your actual connection string
            string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT CustomerID, Name, Email, Username, City, Contact, Address, Gender FROM Customer";

            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        CustomerID = reader.GetInt32(0),
                        Name = reader.GetString(1).Trim(),
                        Email = reader.GetString(2).Trim(),
                        Username = reader.GetString(3).Trim(),
                        City = reader.IsDBNull(4) ? string.Empty : reader.GetString(4).Trim(),
                        Contact = reader.IsDBNull(5) ? string.Empty : reader.GetString(5).Trim(),
                        Address = reader.IsDBNull(6) ? string.Empty : reader.GetString(6).Trim(),
                        Gender = reader.IsDBNull(7) ? string.Empty : reader.GetString(7).Trim()
                    });
                }

                reader.Close();
            }

            CustomersDataGrid.ItemsSource = customers;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            this.Close();
            adminMenu.Show();
        }
    }
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
