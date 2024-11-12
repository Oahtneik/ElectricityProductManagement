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
    /// Interaction logic for CustomerManagerWindow.xaml
    /// </summary>
    public partial class CustomerManagerWindow : Page
    {
        private readonly QlbanHangContext _context;
        public ObservableCollection<ThanhPho> ListOfCategory { get; set; }
        public ObservableCollection<KhachHang> listKhachHang;

        public CustomerManagerWindow()
        {
            _context = new QlbanHangContext();
            InitializeComponent();
            ListOfCategory = new ObservableCollection<ThanhPho>(_context.ThanhPhos.ToList());
            DataContext = this;

        }

        private void DgData_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomer();
            LoadCity();
        }
        private void loadPage()
        {
            using (var context = new QlbanHangContext())
            {
                var thanhPhos = _context.ThanhPhos.ToList();
                ListOfCategory.Clear();
                foreach (var tp in thanhPhos)
                {
                    ListOfCategory.Add(tp);
                }

                var customers = context.KhachHangs.Include(u => u.MaThanhPhoNavigation).Select(c => new KhachHang
                {
                    MaKh = c.MaKh,
                    TenCty = c.TenCty,
                    DienThoai = c.DienThoai,
                    DiaChi = c.DiaChi,
                    MaThanhPho = c.MaThanhPho,
                    MaThanhPhoNavigation = c.MaThanhPhoNavigation,
                }).ToList();
                listKhachHang = new ObservableCollection<KhachHang>(customers);
                DgData.ItemsSource = listKhachHang;
            }

        }
        private void LoadCity()
        {
            var listCities = _context.ThanhPhos.ToList();
            cboThanhpho.ItemsSource = listCities;
            cboThanhpho.DisplayMemberPath = "TenThanhPho";
            cboThanhpho.SelectedValuePath = "MaThanhPho";

        }
        private void LoadCustomer()
        {
            // Load danh sách thành phố
            var thanhPhos = _context.ThanhPhos.ToList();
            ListOfCategory.Clear();
            foreach (var tp in thanhPhos)
            {
                ListOfCategory.Add(tp);
            }

            var customers = _context.KhachHangs.Include(u => u.MaThanhPhoNavigation).Select(c => new KhachHang
            {
                MaKh = c.MaKh,
                TenCty = c.TenCty,
                DienThoai = c.DienThoai,
                DiaChi = c.DiaChi,
                MaThanhPho = c.MaThanhPho,
                MaThanhPhoNavigation = c.MaThanhPhoNavigation,
            }).ToList();
            listKhachHang = new ObservableCollection<KhachHang>(customers);
            DgData.ItemsSource = listKhachHang;

        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgData.SelectedItem is KhachHang customer)
            {
                txtMakh.Text = customer.MaKh;
                txtKhachhang.Text = customer.TenCty;
                txtDiachi.Text = customer.DiaChi;
                cboThanhpho.SelectedValue = customer.MaThanhPho;
                txtDienthoai.Text = customer.DienThoai;
            }
        }

        private void cboThanhpho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMakh.Text = "";
                txtKhachhang.Text = "";
                txtDiachi.Text = "";
                txtDienthoai.Text = "";

                // Kiểm tra và xử lý SelectedValue của ComboBox
                if (cboThanhpho.SelectedItem != null)
                {
                    // Gán SelectedValue nếu có giá trị hợp lệ
                    cboThanhpho.SelectedValue = ((ThanhPho)cboThanhpho.SelectedItem).MaThanhPho;
                }
                else
                {
                    // Xử lý khi không có giá trị được chọn
                    cboThanhpho.SelectedValue = null; // hoặc gán lại giá trị mặc định phù hợp
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var flag = _context.KhachHangs.Where(u => u.MaKh == txtMakh.Text).Any();
                if (!flag)
                {
                    KhachHang khachHang = new KhachHang
                    {
                        MaKh = txtMakh.Text,
                        DiaChi = txtDiachi.Text,
                        DienThoai = txtDienthoai.Text,
                        TenCty = txtKhachhang.Text,
                        MaThanhPho = (int)cboThanhpho.SelectedValue
                    };
                    _context.KhachHangs.Add(khachHang);
                    _context.SaveChanges();
                    listKhachHang.Add(khachHang);
                    MessageBox.Show("Thêm thành công!");
                }
                else
                {
                    MessageBox.Show("Trùng mà khách hàng!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMakh.Text.Length > 0)
                {
                    var khachhang = _context.KhachHangs.Where(u => u.MaKh == txtMakh.Text).FirstOrDefault();
                    if (khachhang != null)
                    {
                        khachhang.MaKh = txtMakh.Text;
                        khachhang.DiaChi = txtDiachi.Text;
                        khachhang.DienThoai = txtDienthoai.Text;
                        khachhang.TenCty = txtKhachhang.Text;
                        khachhang.MaThanhPho = (int)cboThanhpho.SelectedValue;
                        _context.KhachHangs.Update(khachhang);
                        _context.SaveChanges();
                        //DgData.Items.Refresh();
                        MessageBox.Show("Cập nhật thành công!");
                        loadPage();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMakh.Text.Length > 0)
                {
                    var khachhang = _context.KhachHangs.Where(u => u.MaKh == txtMakh.Text).FirstOrDefault();
                    if (khachhang != null)
                    {

                        _context.KhachHangs.Remove(khachhang);
                        _context.SaveChanges();
                        DgData.Items.Refresh();
                        MessageBox.Show("Xóa thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
