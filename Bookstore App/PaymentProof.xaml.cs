using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Bookstore_App
{
    public partial class PaymentProof : Window
    {
        public PaymentProof(string imagePath)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                    paymentImage.Source = new BitmapImage(imageUri);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting image source: " + ex.Message);
                }
            }
            else
            {
                paymentImage.Source = null;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
