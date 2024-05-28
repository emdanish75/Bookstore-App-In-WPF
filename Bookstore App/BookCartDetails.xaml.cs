using System;
using System.Collections.ObjectModel;
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
            MessageBox.Show($"{book.Name} added to cart.");
        }


        private void ShowDetails(Book book)
        {
            MessageBox.Show($"Showing details for {book.Name}.");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ShowCartButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Showing cart.");
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Showing details.");
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

        public int Quantity { get; internal set; }
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
