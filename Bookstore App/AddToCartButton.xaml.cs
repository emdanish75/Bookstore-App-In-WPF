using System;
using System.Data.SqlClient;
using System.Windows;

namespace Bookstore_App
{
    public partial class AddToCartButton : Window
    {
        private const string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
        private BookDetails bookDetails;
        private int customerID;
        private int counter = 1;

        public AddToCartButton(BookDetails bookDetails, int customerID)
        {
            InitializeComponent();
            this.bookDetails = bookDetails;
            this.customerID = customerID;

            bookTitle.Content = bookDetails.Title;
            counterLabel.Content = counter.ToString();
            quantityLabel.Content = counter.ToString();
            priceLabel.Content = (bookDetails.Price * counter).ToString("C");
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            if (counter < bookDetails.Quantity)
            {
                counter++;
                UpdateLabels();
            }
            else
            {
                MessageBox.Show("Quantity of the book cannot be more than the stock.");
            }
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            if (counter > 1)
            {
                counter--;
                UpdateLabels();
            }
            else
            {
                MessageBox.Show("Quantity cannot be less than 1.");
            }
        }

        private void UpdateLabels()
        {
            counterLabel.Content = counter.ToString();
            quantityLabel.Content = counter.ToString();
            priceLabel.Content = (bookDetails.Price * counter).ToString("C");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddToCart();
        }

        private void AddToCart()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    string insertQuery = "INSERT INTO cart (customerID, bookID, quantity, totalBill) VALUES (@CustomerID, (SELECT bookID FROM books WHERE title = @Title), @Quantity, @TotalBill)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction);
                    insertCommand.Parameters.AddWithValue("@CustomerID", customerID);
                    insertCommand.Parameters.AddWithValue("@Title", bookDetails.Title);
                    insertCommand.Parameters.AddWithValue("@Quantity", counter);
                    insertCommand.Parameters.AddWithValue("@TotalBill", bookDetails.Price * counter);
                    insertCommand.ExecuteNonQuery();

                    string updateQuery = "UPDATE books SET quantity = quantity - @Quantity WHERE title = @Title";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction);
                    updateCommand.Parameters.AddWithValue("@Quantity", counter);
                    updateCommand.Parameters.AddWithValue("@Title", bookDetails.Title);
                    updateCommand.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Book added to cart and quantity updated successfully!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error adding book to cart: " + ex.Message);
                }
            }
        }
    }
}