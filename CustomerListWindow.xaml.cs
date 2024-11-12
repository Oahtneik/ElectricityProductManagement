using Microsoft.EntityFrameworkCore;
using SalesManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Page
    {
        private readonly QlbanHangContext _context;
        public ObservableCollection<ThanhPho> ListOfCategory { get; set; }
        public CustomerListWindow()
        {
            _context = new QlbanHangContext();
            InitializeComponent();
            ListOfCategory = new ObservableCollection<ThanhPho>(_context.ThanhPhos.ToList());
            DataContext = this;
        }

        private void DgData_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomer();
        }
        private async void LoadCustomer()
        {
            // Load danh sách thành phố
            var thanhPhos = await _context.ThanhPhos.ToListAsync();
            ListOfCategory.Clear();
            foreach (var tp in thanhPhos)
            {
                ListOfCategory.Add(tp);
            }

            var customers = await _context.KhachHangs.Include(u => u.MaThanhPhoNavigation).Select(c => new KhachHang
            {
                MaKh = c.MaKh,
                TenCty = c.TenCty,
                DiaChi = c.DiaChi,
                DienThoai = c.DienThoai,
                MaThanhPho = c.MaThanhPho,
                MaThanhPhoNavigation = c.MaThanhPhoNavigation,
            }).ToListAsync();
            DgData.ItemsSource = customers;

        }
    }
}
