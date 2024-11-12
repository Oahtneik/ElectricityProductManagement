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
    /// Interaction logic for EditProfilePage.xaml
    /// </summary>
    public partial class EditProfilePage : Page
    {
        private readonly QlbanHangContext _context;
        public EditProfilePage()
        {
            InitializeComponent();
            _context = new QlbanHangContext();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var user = NhanVienSession.NhanVien;
            if (user != null && txtId.Text.Length > 0)
            {
                user.Ten = txtName.Text;
                user.DienThoai = txtPhone.Text;
                user.DiaChi = txtAddress.Text;
                user.NgayNv = DateOnly.Parse(dpDate.Text);
                user.Nu = ((RadioButton)FindName("rbtnFemale")).IsChecked == true ? true : false;
                _context.NhanViens.Update(user);
                _context.SaveChanges();
                MessageBox.Show("Thay đổi thành công");
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = NhanVienSession.NhanVien.MaNv.ToString();
            txtName.Text = NhanVienSession.NhanVien.Ten;
            txtPhone.Text = NhanVienSession.NhanVien.DienThoai;
            txtAddress.Text = NhanVienSession.NhanVien.DiaChi;
            if (NhanVienSession.NhanVien.Nu == true)
            {
                ((RadioButton)FindName("rbtnFemale")).IsChecked = true;
            }
            else
            {
                ((RadioButton)FindName("rbtnMale")).IsChecked = true;
            }
            dpDate.Text = NhanVienSession.NhanVien.NgayNv.ToString();
        }
    }
}
