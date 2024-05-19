using System;
using System.Data.SqlClient;
using System.Windows;

namespace Bookstore_App
{
    public partial class CustomerDiaglog : Window
    {
        private const string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
        private string nameC;
        private string emailC;
        private string usernameC;
        private int customerID;

        public CustomerDiaglog(string name, string email, string username)
        {
            InitializeComponent();
            nameC = name;
            emailC = email;
            usernameC = username;
            nextButton.IsEnabled = false;   
            // Generate a random 4-digit integer for customerID
            Random rand = new Random();
            customerID = rand.Next(1000, 9999);

            // Check if customer details are already present for the given username
            CheckCustomerDetails();
        }

        private void CheckCustomerDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT city, contact, address, gender FROM customer WHERE username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", usernameC);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string city = reader.IsDBNull(0) ? null : reader.GetString(0);
                            string contact = reader.IsDBNull(1) ? null : reader.GetString(1);
                            string address = reader.IsDBNull(2) ? null : reader.GetString(2);
                            string gender = reader.IsDBNull(3) ? null : reader.GetString(3);

                            if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(contact) &&
                                !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(gender))
                            {
                                // Customer details are already present, disable the register button
                                registerBtn.IsEnabled = false;
                                nextButton.IsEnabled = true;

                                // Fill the textboxes with existing customer details
                                cityTextBox.Text = city;
                                contactTexbox.Text = contact;
                                addressTextBox.Text = address;
                                if (gender == "Male")
                                    maleRadio.IsChecked = true;
                                else
                                    femaleRadio.IsChecked = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking customer details: " + ex.Message);
                }
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string city = cityTextBox.Text;
            string contact = contactTexbox.Text;
            string address = addressTextBox.Text;
            string gender = maleRadio.IsChecked == true ? "Male" : "Female";

            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Insert the customer details into the database
            InsertCustomerDetails(city, contact, address, gender);
        }

        private void InsertCustomerDetails(string city, string contact, string address, string gender)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO customer (customerID, name, email, username, city, contact, address, gender) VALUES (@CustomerID, @Name, @Email, @Username, @City, @Contact, @Address, @Gender)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@Name", nameC);
                    command.Parameters.AddWithValue("@Email", emailC);
                    command.Parameters.AddWithValue("@Username", usernameC);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Customer added successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding customer: " + ex.Message);
                }
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open next page");
        }
    }
}
