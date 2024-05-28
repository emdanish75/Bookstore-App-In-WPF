using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore_App
{
    public partial class Checkout : Window
    {
        private int customerId;
        private int totalBill;

        public Checkout(int customerId, int totalBill)
        {
            InitializeComponent();
            this.customerId = customerId;
            this.totalBill = totalBill;
            totalBillLabel.Content = $"${totalBill}";
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|.jpg;.jpeg;.png;.bmp;*.gif",
                Title = "Select a Payment Proof"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Assuming the file upload process happens here.
                uploadSuccessLabel.Visibility = Visibility.Visible;
                uploadButton.Visibility = Visibility.Collapsed;
                checkoutButton.IsEnabled = true;
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checkout successful!");
            this.Close();
        }
    }
}