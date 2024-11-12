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
    /// Interaction logic for CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Page
    {
        private readonly QlbanHangContext _context;
        public ObservableCollection<KhachHang> listkhachHangs { get; set; }
        public ObservableCollection<OrderDetail> OrderDetails { get; set; }
        public ObservableCollection<SanPham> ListOfCategory { get; set; }
        public CreateOrderWindow()
        {
            InitializeComponent();
            _context = new QlbanHangContext();
            listkhachHangs = new ObservableCollection<KhachHang>(_context.KhachHangs.ToList());
            ListOfCategory = new ObservableCollection<SanPham>(_context.SanPhams.ToList());
            cbokhachang.ItemsSource = listkhachHangs;
            cbokhachang.DisplayMemberPath = "TenCty";
            cbokhachang.SelectedValuePath = "MaKh";
            OrderDetails = new ObservableCollection<OrderDetail>();
            DataContext = this;
        }

        private void cbokhachang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbokhachang.SelectedValue != null)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        SanPham = "Default Product",
                        SoLuong = 1,
                        DonGia = 100,
                        ThanhTien = 100
                    };

                    OrderDetails.Add(newOrderDetail);
                }
                var lastHoaDon = _context.HoaDons
                                        .OrderByDescending(hd => hd.MaHd)
                                        .FirstOrDefault();

                if (lastHoaDon != null)
                {
                    int newMaHd = lastHoaDon.MaHd + 1;
                    txtMahd.Text = newMaHd.ToString();
                }
                else
                {
                    txtMahd.Text = "1"; // Nếu không có hóa đơn nào, bắt đầu từ 1
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox && comboBox.SelectedValue != null)
                {
                    var selectedProductId = (int)comboBox.SelectedValue;
                    var selectedProduct = await _context.SanPhams.FindAsync(selectedProductId);

                    if (selectedProduct != null && DgData.SelectedItem is OrderDetail selectedOrderDetail)
                    {
                        selectedOrderDetail.DonGia = (decimal)selectedProduct.DonGia;
                        selectedOrderDetail.ThanhTien = selectedOrderDetail.SoLuong * (decimal)selectedProduct.DonGia;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var dataGrid = (DataGrid)sender;
                var orderDetail = (OrderDetail)dataGrid.SelectedItem;
                if (orderDetail != null)
                {
                    var sanPham = ListOfCategory.FirstOrDefault(sp => sp.MaSp == orderDetail.MaSp);
                    if (sanPham != null)
                    {
                        orderDetail.DonGia = (decimal)sanPham.DonGia;
                        orderDetail.ThanhTien = orderDetail.SoLuong * orderDetail.DonGia;
                    }
                }
            }
        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var o in OrderDetails)
                {
                    if (o.SoLuong < 0)
                    {
                        MessageBox.Show("Số lượng không được nhỏ hơn 0", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Ngừng thực hiện nếu phát hiện số lượng âm
                    }
                }
                var currentEmployee = NhanVienSession.NhanVien;
                var lastOrder = _context.ChiTietHoaDons.OrderByDescending(m => m.MaHd).FirstOrDefault();
                int lastId = lastOrder.MaHd + 1;
                // Tạo đối tượng HoaDon mới
                var hoaDon = new HoaDon
                {
                    MaKh = (string)cbokhachang.SelectedValue,
                    NgayLapHd = DateTime.Now,
                    NgayNhanHang = DateTime.Now.AddDays(2),
                    MaNv = currentEmployee.MaNv
                };

                // Thêm HoaDon vào context
                _context.HoaDons.Add(hoaDon);
                _context.SaveChanges(); // Lưu để có MaHd cho ChiTietHoaDon

                // Tạo đối tượng ChiTietHoaDon từ OrderDetails
                foreach (var o in OrderDetails)
                {
                    var chiTietHoaDon = new ChiTietHoaDon
                    {
                        MaHd = lastId,
                        MaSp = o.MaSp,
                        SoLuong = o.SoLuong,
                    };

                    // Thêm ChiTietHoaDon vào context
                    _context.ChiTietHoaDons.Add(chiTietHoaDon);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                // Clear the ComboBox selection
                cbokhachang.SelectedValue = null;

                // Clear the DataGrid items
                OrderDetails.Clear();

                // Optionally, reset the order number text box
                txtMahd.Text = string.Empty;
                MessageBox.Show("Tạo hóa đơn thành công");
            }
            catch (DbUpdateException dbEx)
            {
                // Hiển thị chi tiết lỗi
                MessageBox.Show($"DbUpdateException: {dbEx.Message}\n\nInner Exception: {dbEx.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Hiển thị chi tiết lỗi
                MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
