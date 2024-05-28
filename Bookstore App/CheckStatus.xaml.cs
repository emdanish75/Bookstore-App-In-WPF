using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Bookstore_App
{
    public partial class CheckStatus : Window
    {
        public ObservableCollection<BookItem> BookItems { get; set; }
        private int customerId; // Assuming you have a way to get the current customer's ID

        public CheckStatus(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            BookItems = new ObservableCollection<BookItem>();
            bookList.ItemsSource = BookItems;
            LoadBookItems();
        }

        private void LoadBookItems()
        {
            string connectionString = "Data Source=DEVELOPER-966\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT b.title, b.pdfPath FROM cart c JOIN books b ON c.bookID = b.bookID WHERE c.customerID = @customerID";

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
                        BookItems.Add(new BookItem
                        {
                            Title = reader.GetString(0).Trim(),
                            PdfPath = reader.GetString(1).Trim()
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading book items: " + ex.Message);
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookList.SelectedItem is BookItem selectedItem)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = System.IO.Path.GetFileName(selectedItem.PdfPath),
                    DefaultExt = ".pdf",
                    Filter = "PDF documents (.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        System.IO.File.Copy(selectedItem.PdfPath, saveFileDialog.FileName, true);
                        MessageBox.Show("File downloaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error downloading file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            downloadButton.IsEnabled = bookList.SelectedItem != null;
        }
    }

    public class BookItem
    {
        public string Title { get; set; }
        public string PdfPath { get; set; }
    }
}