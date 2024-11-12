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
    /// Interaction logic for OrderManageWindow.xaml
    /// </summary>
    public partial class OrderManageWindow : Page
    {
        private readonly QlbanHangContext _context;
        public ObservableCollection<ThanhPho> ListOfThanhPho { get; set; }
        public ObservableCollection<KhachHang> ListOfKhachHang { get; set; }
        public OrderManageWindow()
        {
            InitializeComponent();
            _context = new QlbanHangContext();
            ListOfThanhPho = new ObservableCollection<ThanhPho>(_context.ThanhPhos.ToList());
            cboThanhpho.ItemsSource = ListOfThanhPho;
            cboThanhpho.DisplayMemberPath = "TenThanhPho";
            cboThanhpho.SelectedValuePath = "MaThanhPho";
        }

        private void cboThanhpho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboThanhpho.SelectedValue != null)
            {
                int selectedCityId = (int)cboThanhpho.SelectedValue;
                ListOfKhachHang = new ObservableCollection<KhachHang>(_context.KhachHangs
                    .Where(kh => kh.MaThanhPho == selectedCityId).ToList());
                cboKhachhang.ItemsSource = ListOfKhachHang;
                cboKhachhang.DisplayMemberPath = "TenCty";
                cboKhachhang.SelectedValuePath = "MaKh";
                txtSlKhachhang.Text = ListOfKhachHang.Count.ToString();
            }
        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DgData.SelectedItem is HoaDonDTO hoaDon)
                {
                    var orderDetail = _context.ChiTietHoaDons.Include(c => c.MaSpNavigation)
                        .Where(c => c.MaHd == hoaDon.MaHd).Select(c => new
                        {
                            c.MaHd,
                            SanPham = c.MaSpNavigation.TenSp,
                            c.SoLuong,
                            DonGia = c.MaSpNavigation.DonGia,
                            ThanhTien = c.MaSpNavigation.DonGia * c.SoLuong

                        }).ToList();
                    DgData2.ItemsSource = orderDetail;
                    txtSlMathang.Text = orderDetail.Count.ToString();
                }
                else
                {
                    MessageBox.Show("Selected item is not of type HoaDon");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void cboKhachhang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboKhachhang.SelectedValue != null)
            {
                string makh = cboKhachhang.SelectedValue.ToString();
                var listOrder = await _context.HoaDons
                                              .Include(hd => hd.MaKhNavigation)

                                              .Where(hd => hd.MaKh == makh)
                                              .Select(hd => new HoaDonDTO
                                              {
                                                  MaHd = hd.MaHd,
                                                  KhachHang = hd.MaKhNavigation.TenCty,
                                                  NgayLapHd = hd.NgayLapHd,
                                                  NgayNhanHang = hd.NgayNhanHang,
                                                  NhanVien = hd.MaNvNavigation.Ten
                                              })
                                              .ToListAsync();
                DgData.ItemsSource = listOrder;
                txtSlHoadon.Text = listOrder.Count.ToString();
            }
        }

    }
}
