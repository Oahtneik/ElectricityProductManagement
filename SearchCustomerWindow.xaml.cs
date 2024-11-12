using Microsoft.EntityFrameworkCore;
using SalesManager.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for SearchCustomerWindow.xaml
    /// </summary>
    public partial class SearchCustomerWindow : Page
    {
        private readonly QlbanHangContext _context;

        public SearchCustomerWindow()
        {

            _context = new QlbanHangContext();

            InitializeComponent();
            LoadCities();
        }
        private async void LoadCities()
        {
            var cities = await _context.ThanhPhos.ToListAsync();
            cboCity.ItemsSource = cities;
            cboCity.DisplayMemberPath = "TenThanhPho";
            cboCity.SelectedValuePath = "MaThanhPho";
        }
        private async void cboCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCity.SelectedItem is ThanhPho selectedCity)
            {
                var customers = await _context.KhachHangs.Where(c => c.MaThanhPho == selectedCity.MaThanhPho).ToListAsync();
                DgData.ItemsSource = customers;
                txtCount.Text = customers.Count.ToString();
            }

        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
