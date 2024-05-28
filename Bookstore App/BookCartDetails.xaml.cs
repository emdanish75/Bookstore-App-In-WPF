using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Bookstore_App
{
    public partial class BookCartDetails : Window
    {
        public ObservableCollection<Book> Books { get; set; }
    public ICommand AddToCartCommand { get; set; }
    public ICommand ShowDetailsCommand { get; set; }
    private int customerID;

    public BookCartDetails(int userID)
    {
        InitializeComponent();
        DataContext = this;
        customerID = userID;

        Books = new ObservableCollection<Book>();

        AddToCartCommand = new RelayCommand<Book>(AddToCart);
        ShowDetailsCommand = new RelayCommand<Book>(ShowDetails);

        LoadBooks();
    }

        private void LoadBooks()
        {
            string connectionString = "Data Source=DANISH-HP-LAPTO\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT title, price, imagePath FROM books WHERE quantity >= 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string title = reader.GetString(0);
                        double price = reader.GetInt32(1);  // Changed to GetDouble to match the data type
                        string imagePath = reader.GetString(2);

                        Books.Add(new Book { Name = title, Price = price, PicturePath = imagePath });
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching books: " + ex.Message);
            }
        }


        private void AddToCart(Book book)
        {
            BookDetails bookDetails = FetchBookDetailsFromDatabase(book.Name);
            AddToCartButton addToCartButton = new AddToCartButton(bookDetails, customerID);  // Pass customerID
            addToCartButton.Show();
        }


        private void ShowDetails(Book book)
        {
            BookDetails bookDetails = FetchBookDetailsFromDatabase(book.Name);
            if (bookDetails != null)
            {
                ViewBookDetailsCustomerSide viewDetailsPage = new ViewBookDetailsCustomerSide(bookDetails);
                viewDetailsPage.Show();
            }
        }

        private BookDetails FetchBookDetailsFromDatabase(string bookTitle)
        {
            string connectionString = "Data Source=DANISH-HP-LAPTO\\SQLEXPRESS;Initial Catalog=projectdb;Integrated Security=True;";
            string query = "SELECT title, genre, quantity, price, description, imagePath FROM books WHERE title = @title";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", bookTitle);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new BookDetails
                                {
                                    Title = reader.GetString(0),
                                    Genre = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    Price = reader.GetInt32(3),
                                    Description = reader.GetString(4),
                                    ImagePath = reader.GetString(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching book details: " + ex.Message);
            }
            return null;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ShowCartButton_Click(object sender, RoutedEventArgs e)
        {
            showCart showCart = new showCart(customerID);
            showCart.Show();
            this.Close();
        }

        private void ViewOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Viewing orders.");
        }
    }

    // Book class remains unchanged
    public class Book
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string PicturePath { get; set; }

        private BitmapImage picture;
        public BitmapImage Picture
        {
            get
            {
                if (picture == null)
                {
                    try
                    {
                        Uri imageUri = new Uri(PicturePath, UriKind.Absolute);
                        picture = new BitmapImage(imageUri);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                        picture = new BitmapImage(); // You could set this to a default image URI if you have one
                    }
                }
                return picture;
            }
        }
    }
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    // RelayCommand class remains unchanged
}
