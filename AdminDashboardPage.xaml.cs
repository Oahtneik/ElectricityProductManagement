using Microsoft.VisualBasic.ApplicationServices;
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
using System.Windows.Shapes;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for AdminDashboardPage.xaml
    /// </summary>
    public partial class AdminDashboardPage : Window
    {
        private readonly QlbanHangContext _context;
        public List<StatusOption> StatusOptions { get; set; }
        public AdminDashboardPage()
        {
            InitializeComponent();
            _context = new QlbanHangContext();
           
            StatusOptions = new List<StatusOption>
        {
        new StatusOption { Value = true, Text = "Khóa" },
        new StatusOption { Value = false, Text = "Hoạt động" }
        };
            DataContext = this;
            cbTrangThai.DisplayMemberPath = "Text";
            cbTrangThai.SelectedValuePath = "Value";
        }
        public class StatusOption
        {
            public bool Value { get; set; }
            public string Text { get; set; }
        }
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var list = _context.NhanViens.Where(nv => !nv.IsAdmin).ToList();
            DgData.ItemsSource = list;
        }

        private void loadPage()
        {
            using (var context = new QlbanHangContext())
            {
                var list = context.NhanViens.Where(nv => !nv.IsAdmin).ToList();
                DgData.ItemsSource = list;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var updatedMembers = DgData.ItemsSource as List<NhanVien>;
            if (updatedMembers != null)
            {
                using (var context = new QlbanHangContext())
                {
                    foreach (var member in updatedMembers)
                    {
                        var dbMember = context.NhanViens.Find(member.MaNv);
                        if (dbMember != null)
                        {
                            dbMember.status = member.status;
                        }
                    }
                    context.SaveChanges();
                }
                MessageBox.Show("Đã cập nhật trạng thái của tất cả thành viên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Tạo một đối tượng NhanVien mới
                var newEmployee = new NhanVien
                {
                    //MaNv = int.Parse(txtMaNv.Text), // Hoặc dùng auto-increment nếu MaNv không phải là nhập tay
                    Ho = txtHo.Text,
                    Ten = txtTen.Text,

                    DienThoai = txtDienthoai.Text,
                    MaDn = txtMaDn.Text,
                    MatKhau = txtMatKhau.Text,
                    DiaChi = txtDiachi.Text,
                    NgayNv = dpDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDate.SelectedDate.Value) : (DateOnly?)null,
                    status = (bool)cbTrangThai.SelectedValue, // Giả sử cbTrangThai chứa các giá trị bool
                    IsAdmin = false, // Hoặc giá trị mặc định nào đó
                    Nu = rbtnFemale.IsChecked == true
            };

                // Xác định giới tính từ RadioButton
                //newEmployee.Nu = rbtnFemale.IsChecked == true;

                // Thêm đối tượng vào cơ sở dữ liệu
                using (var context = new QlbanHangContext())
                {
                    if (context.NhanViens.Any(nv => nv.MaDn == txtMaDn.Text))
                    {
                        MessageBox.Show("Mã đăng nhập đã tồn tại. Vui lòng chọn mã khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    context.NhanViens.Add(newEmployee);
                    context.SaveChanges();
                }

                // Thông báo thành công
                MessageBox.Show("Nhân viên đã được thêm thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cập nhật lại DataGrid sau khi thêm mới
                DataGrid_Loaded(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy MaNv từ TextBox
                if (int.TryParse(txtMaNv.Text, out int maNv))
                {
                    using (var context = new QlbanHangContext())
                    {
                        // Tìm nhân viên cần cập nhật
                        var existingEmployee = context.NhanViens.FirstOrDefault(nv => nv.MaNv == maNv);

                        if (existingEmployee != null)
                        {
                            if (context.NhanViens.Any(nv => nv.MaDn == txtMaDn.Text && nv.MaNv != maNv))
                            {
                                MessageBox.Show("Mã đăng nhập đã tồn tại. Vui lòng chọn mã khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            // Cập nhật thông tin nhân viên
                            existingEmployee.Ho = txtHo.Text;
                            existingEmployee.Ten = txtTen.Text;
                            existingEmployee.DienThoai = txtDienthoai.Text;
                            existingEmployee.MaDn = txtMaDn.Text;
                            existingEmployee.MatKhau = txtMatKhau.Text;
                            existingEmployee.DiaChi = txtDiachi.Text;
                            existingEmployee.NgayNv = dpDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDate.SelectedDate.Value) : (DateOnly?)null;
                            existingEmployee.status = (bool)cbTrangThai.SelectedValue;
                            existingEmployee.Nu = rbtnFemale.IsChecked == true;

                            context.NhanViens.Update(existingEmployee);
                            // Lưu thay đổi vào cơ sở dữ liệu
                            context.SaveChanges();

                            // Thông báo thành công
                            MessageBox.Show("Nhân viên đã được cập nhật thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Cập nhật lại DataGrid sau khi cập nhật
                            
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên với mã này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    loadPage();
                }
                else
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTrangThai()
        {
            var listTrangThai = _context.NhanViens.ToList();
            cbTrangThai.ItemsSource = listTrangThai;
            cbTrangThai.DisplayMemberPath = "Text";
            cbTrangThai.SelectedValuePath = "Value";


        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgData.SelectedItem is NhanVien nhanvien)
            {
                txtMaNv.Text = nhanvien.MaNv.ToString();
                txtHo.Text = nhanvien.Ho;
                txtTen.Text = nhanvien.Ten;
                txtDiachi.Text = nhanvien.DiaChi;
                cbTrangThai.SelectedValue = nhanvien.status;
                txtDienthoai.Text = nhanvien.DienThoai;
                txtMaDn.Text=nhanvien.MaDn;
                txtMatKhau.Text=nhanvien.MatKhau;
                if (nhanvien.Nu == true)
                {
                    ((RadioButton)FindName("rbtnFemale")).IsChecked = true;
                }
                else
                {
                    ((RadioButton)FindName("rbtnMale")).IsChecked = true;
                }
                dpDate.Text = nhanvien.NgayNv.ToString();

            }

        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMaNv.Text = "";
                txtDiachi.Text = "";
                txtDienthoai.Text = "";
                txtHo.Text = "";
                txtTen.Text = "";
                txtMaDn.Text = "";
                txtMatKhau.Text = "";
                cbTrangThai.SelectedValue = null;
                rbtnMale.IsChecked = false;
                rbtnFemale.IsChecked = false;
                dpDate.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            // Hiển thị MainWindow
            mainWindow.Show();
        }
    }
}
