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
    
    public partial class AdminMenu : Window
    {
        public AdminMenu()
        {
            InitializeComponent();
        }
        private void bookButton_Click(object sender, RoutedEventArgs e)
        {
            bookSideActivity bookActivity = new bookSideActivity();
            this.Close();
            bookActivity.Show();
        }

        private void bookButton_Copy2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void customerButton_Click(object sender, RoutedEventArgs e)
        {
            ViewCustomerDetails viewCustomerDetail = new ViewCustomerDetails();
            this.Close();
            viewCustomerDetail.Show();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            ViewOrdersAdmin viewOrdersAdmin = new ViewOrdersAdmin();
            this.Close();
            viewOrdersAdmin.Show();
        }
    }
}
