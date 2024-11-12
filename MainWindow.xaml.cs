using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CustomerListButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomerListWindow());
        }

        private void CustomerManageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomerManagerWindow());
        }

        private void CustomerSearchButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SearchCustomerWindow());
        }

        private void OrderManageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrderManageWindow());
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CreateOrderWindow());
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditProfilePage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListProduct());
        }
    }
}