using System;
using System.Collections.Generic;

namespace SalesManager.Models;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string TenCty { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public int MaThanhPho { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ThanhPho MaThanhPhoNavigation { get; set; } = null!;
}
