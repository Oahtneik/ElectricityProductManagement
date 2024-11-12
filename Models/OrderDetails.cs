using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.Models
{
    public class OrderDetail : System.ComponentModel.INotifyPropertyChanged
    {
        private int soLuong;
        private decimal donGia;
        private decimal thanhTien;

        public int MaSp { get; set; }
        public string SanPham { get; set; }

        public int SoLuong
        {
            get { return soLuong; }
            set
            {
                if (soLuong != value)
                {
                    soLuong = value;
                    OnPropertyChanged(nameof(SoLuong));
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public decimal DonGia
        {
            get { return donGia; }
            set
            {
                if (donGia != value)
                {
                    donGia = value;
                    OnPropertyChanged(nameof(DonGia));
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public decimal ThanhTien
        {
            get { return soLuong * donGia; }
            set
            {
                if (thanhTien != value)
                {
                    thanhTien = value;
                    OnPropertyChanged(nameof(ThanhTien));
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
