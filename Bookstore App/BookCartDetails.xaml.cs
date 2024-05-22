using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Bookstore_App
{
    /// <summary>
    /// Interaction logic for BookCartDetails.xaml
    /// </summary>
    public partial class BookCartDetails : Window
    {
        public ObservableCollection<Book> Books { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand ShowDetailsCommand { get; set; }

        public BookCartDetails()
        {
            InitializeComponent();
            DataContext = this;

            // Sample data for books
            Books = new ObservableCollection<Book>
            {
                new Book { Name = "Book 1", Price = 19.99, PicturePath = "C:\\Users\\BISMILLAH COMPUTERS\\Downloads\\ICONS\\latte.jpg" },
                new Book { Name = "Book 2", Price = 29.99, PicturePath = "C:\\Users\\BISMILLAH COMPUTERS\\Downloads\\ICONS\\cup.png" },
                new Book { Name = "Book 3", Price = 29.99, PicturePath = "C:\\Users\\BISMILLAH COMPUTERS\\Downloads\\ICONS\\images (1).png" },
                new Book { Name = "Book 4", Price = 29.99, PicturePath = "C:\\Users\\BISMILLAH COMPUTERS\\Downloads\\ICONS\\staff.png" },
                new Book { Name = "Book 5", Price = 29.99, PicturePath = "C:\\Users\\BISMILLAH COMPUTERS\\Downloads\\ICONS\\espresso.png" },

                // Add more books as needed
            };

            AddToCartCommand = new RelayCommand<Book>(AddToCart);
            ShowDetailsCommand = new RelayCommand<Book>(ShowDetails);
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
                        // Handle the exception (e.g., log it, show a default image, etc.)
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
}
