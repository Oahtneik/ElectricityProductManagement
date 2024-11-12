using System;
using System.Collections.Generic;

namespace SalesManager.Models;

public partial class ChiTietHoaDon
{
    public int MaHd { get; set; }

    public int MaSp { get; set; }

    public int SoLuong { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
