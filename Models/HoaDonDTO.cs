using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.Models
{
    class HoaDonDTO
    {
        public int MaHd { get; set; }
        public string KhachHang { get; set; }
        public DateTime NgayLapHd { get; set; }
        public DateTime NgayNhanHang { get; set; }
        public string NhanVien { get; set; }
    }
}
