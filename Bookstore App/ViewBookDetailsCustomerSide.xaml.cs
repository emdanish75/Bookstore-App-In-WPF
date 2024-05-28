using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bookstore_App
{
    public partial class ViewBookDetailsCustomerSide : Window
    {
        public ViewBookDetailsCustomerSide(BookDetails bookDetails)
        {
            InitializeComponent();
            // Set the labels with the book details
            titleLabel_Copy.Content = bookDetails.Title;
            genreLabel_Copy.Content = bookDetails.Genre;
            quantityLabel_Copy.Content = bookDetails.Quantity.ToString();
            priceLabel_Copy.Content = $"${bookDetails.Price}";
            descriptionLabel_Copy.Content = bookDetails.Description;

            // Set the image source if an image path is available
            if (!string.IsNullOrEmpty(bookDetails.ImagePath))
            {
                try
                {
                    Uri imageUri = new Uri(bookDetails.ImagePath, UriKind.Absolute);
                    booksImages.Source = new BitmapImage(imageUri);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting image source: " + ex.Message);
                }
            }
            else
            {
                // Clear the image source if no image path is found
                booksImages.Source = null;
            }
        }
    }
 }

