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
                Title = "Select a Payment Proof",
                Filter = "Image Files|*.jpg;*.jpeg;*.png",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                string fileExtension = System.IO.Path.GetExtension(selectedFile).ToLower();

                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    uploadSuccessLabel.Visibility = Visibility.Visible;
                    uploadButton.Visibility = Visibility.Collapsed;
                    checkoutButton.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Invalid image format. Please choose a jpg or png file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checkout successful!");
            this.Close();
        }
    }
}