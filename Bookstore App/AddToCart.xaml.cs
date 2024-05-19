using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bookstore_App
{
    public partial class AddToCart : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books { get; set; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveBookCommand { get; }

        public int TotalQuantity
        {
            get { return Books.Sum(book => book.Quantity); }
        }

        public double TotalPrice
        {
            get { return Books.Sum(book => book.Quantity * book.Price); }
        }

        public AddToCart()
        {
            InitializeComponent(); // Ensure this is called

            DataContext = this;

            Books = new ObservableCollection<Book>
            {
                new Book { Name = "Book 1", Price = 10.99, Picture = "Images/book1.png", Quantity = 0 },
                new Book { Name = "Book 2", Price = 15.49, Picture = "Images/book2.png", Quantity = 0 },
                new Book { Name = "Book 3", Price = 7.99, Picture = "Images/book3.png", Quantity = 0 }
            };

            IncreaseQuantityCommand = new RelayCommand<Book>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<Book>(DecreaseQuantity);
            RemoveBookCommand = new RelayCommand<Book>(RemoveBook);
        }

        private void IncreaseQuantity(Book book)
        {
            book.Quantity++;
            OnPropertyChanged(nameof(TotalQuantity));
            OnPropertyChanged(nameof(TotalPrice));
        }

        private void DecreaseQuantity(Book book)
        {
            if (book.Quantity > 0)
            {
                book.Quantity--;
                OnPropertyChanged(nameof(TotalQuantity));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private void RemoveBook(Book book)
        {
            Books.Remove(book);
            OnPropertyChanged(nameof(TotalQuantity));
            OnPropertyChanged(nameof(TotalPrice));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

       
    }

    // RelayCommand class implementation
    
}
