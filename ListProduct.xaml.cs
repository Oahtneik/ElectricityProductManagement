using Microsoft.EntityFrameworkCore;
using SalesManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalesManager
{
    public partial class ListProduct : Page
    {
        private readonly QlbanHangContext _context;
        public ObservableCollection<SanPham> listSanPham;

        public ListProduct()
        {
            _context = new QlbanHangContext();
            InitializeComponent();
            LoadProducts();
            DataContext = this;
        }

        private void LoadProducts()
        {
            var productListWrapper = new ProductListWrapper(_context);
            var productListForNavigation = productListWrapper.GetProductListForNavigation();
            listSanPham = new ObservableCollection<SanPham>((IEnumerable<SanPham>)productListForNavigation);
            DgProductData.ItemsSource = listSanPham;
        }

        private void DgProductData_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void DgProductData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgProductData.SelectedItem is SanPham product)
            {
                txtMaSp.Text = product.MaSp.ToString();
                txtTenSp.Text = product.TenSp;
                txtDonViTinh.Text = product.DonViTinh;
                txtDonGia.Text = product.DonGia?.ToString();
                // Nếu trường hình không null, gán vào trường Hình
                txtHinh.Text = product.Hinh ?? string.Empty;  // Mặc định nếu Hinh là null thì để trống
            }
        }


        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int maSp = int.Parse(txtMaSp.Text);
                var flag = _context.SanPhams.Any(u => u.MaSp == maSp);

                if (!flag)
                {
                    var product = new SanPham
                    {
                        MaSp = maSp,
                        TenSp = txtTenSp.Text,
                        DonViTinh = txtDonViTinh.Text,
                        DonGia = double.TryParse(txtDonGia.Text, out double donGia) ? donGia : 0,
                        Hinh = null  // Mặc định trường Hinh là null
                    };

                    _context.SanPhams.Add(product);
                    _context.SaveChanges();
                    listSanPham.Add(product);
                    MessageBox.Show("Thêm thành công!");
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtMaSp.Text, out int maSp))
                {
                    var product = _context.SanPhams.FirstOrDefault(p => p.MaSp == maSp);
                    if (product != null)
                    {
                        product.TenSp = txtTenSp.Text;
                        product.DonViTinh = txtDonViTinh.Text;
                        product.DonGia = double.TryParse(txtDonGia.Text, out double donGia) ? donGia : 0;
                        product.Hinh = null;  // Nếu không có hình, mặc định Hinh là null

                        _context.SanPhams.Update(product);
                        _context.SaveChanges();
                        LoadProducts();
                        MessageBox.Show("Cập nhật thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtMaSp.Text, out int maSp))
                {
                    var product = _context.SanPhams.FirstOrDefault(p => p.MaSp == maSp);
                    if (product != null)
                    {
                        _context.SanPhams.Remove(product);
                        _context.SaveChanges();
                        listSanPham.Remove(product);
                        MessageBox.Show("Xóa thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelProduct_Click(object sender, RoutedEventArgs e)
        {
            ClearTextFields();
        }

        private void ClearTextFields()
        {
            txtMaSp.Clear();
            txtTenSp.Clear();
            txtDonViTinh.Clear();
            txtDonGia.Clear();
        }
    }
}
