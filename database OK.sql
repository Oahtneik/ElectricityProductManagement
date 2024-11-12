CREATE DATABASE QLBanHang;
GO

USE QLBanHang;
GO

CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY IDENTITY,
    Ho VARCHAR(50),
    Ten VARCHAR(50),
    Nu BIT,
    NgayNV DATE,
    DiaChi VARCHAR(50),
    DienThoai VARCHAR(50),
    Hinh VARCHAR(50),
    MaDN VARCHAR(50) NOT NULL,
    status BIT NOT NULL,
    isAdmin BIT NOT NULL,
    MatKhau VARCHAR(50) NOT NULL
);

CREATE TABLE [SanPham] (
    [MaSP] INT PRIMARY KEY IDENTITY,
    [TenSP] NVARCHAR(40) NOT NULL,
    [DonViTinh] NVARCHAR(50) NULL,
    [DonGia] FLOAT NULL DEFAULT 0.0,
    [Hinh] NVARCHAR(50) NULL
);

CREATE TABLE [ThanhPho] (
    [MaThanhPho] INT PRIMARY KEY IDENTITY,
    [TenThanhPho] NVARCHAR(50) NULL
);

CREATE TABLE [KhachHang] (
    [MaKH] NVARCHAR(20) PRIMARY KEY,
    [TenCty] NVARCHAR(50) NOT NULL,
    [DiaChi] NVARCHAR(60) NULL,
    [DienThoai] NVARCHAR(24) NULL,
    [MaThanhPho] INT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_KhachHang_ThanhPho] FOREIGN KEY ([MaThanhPho]) REFERENCES [ThanhPho] ([MaThanhPho]) ON DELETE CASCADE
);

CREATE TABLE [HoaDon] (
    [MaHD] INT PRIMARY KEY IDENTITY,
    [MaKH] NVARCHAR(20) NOT NULL,
    [NgayLapHD] DATETIME NOT NULL DEFAULT (GETDATE()),
    [NgayNhanHang] DATETIME NOT NULL DEFAULT ('1900-01-01'),
    [MaNV] INT NULL,
    CONSTRAINT [FK_HoaDon_NhanVien] FOREIGN KEY ([MaNV]) REFERENCES [NhanVien] ([MaNV]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([MaKH]) REFERENCES [KhachHang] ([MaKH])
);

CREATE TABLE [ChiTietHoaDon] (
    [MaHD] INT NOT NULL,
    [MaSP] INT NOT NULL,
    [SoLuong] INT NOT NULL DEFAULT 1,
    PRIMARY KEY ([MaHD], [MaSP]),
    CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY ([MaHD]) REFERENCES [HoaDon] ([MaHD]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY ([MaSP]) REFERENCES [SanPham] ([MaSP])
);

CREATE INDEX [IX_ChiTietHoaDon_MaSP] ON [ChiTietHoaDon] ([MaSP]);
CREATE INDEX [IX_HoaDon_MaKH] ON [HoaDon] ([MaKH]);
CREATE INDEX [IX_HoaDon_MaNV] ON [HoaDon] ([MaNV]);
CREATE INDEX [IX_KhachHang_MaThanhPho] ON [KhachHang] ([MaThanhPho]);

-- Insert data into ThanhPho
INSERT INTO ThanhPho (TenThanhPho)
VALUES
('Ha Noi'),
('Ho Chi Minh'),
('Da Nang');

-- Insert data into KhachHang
INSERT INTO KhachHang (MaKH, TenCty, DiaChi, DienThoai, MaThanhPho)
VALUES
('KH001', 'Nguyen Van D', '123 Duong A, Ha Noi', '0123456789', 1),
('KH002', 'Vu Thi E', '456 Duong B, Ho Chi Minh', '0987654321', 2),
('KH003', 'Anh F', '789 Duong C, Da Nang', '0123987654', 3);

-- Insert data into NhanVien
INSERT INTO NhanVien (Ho, Ten, Nu, NgayNV, DiaChi, DienThoai, Hinh, MaDN, status, isAdmin, MatKhau)
VALUES
('Nguyen', 'Van A', 0, '2022-01-01', '123 Street', '0123456789', 'imageA.jpg', 'admin', 0, 1, 'passwordA'),
('Tran', 'Thi B', 1, '2023-02-01', '456 Street', '0987654321', 'imageB.jpg', 'admin1', 1, 1, 'passwordB'),
('Nguyen', 'Van A', 0, '2022-01-01', '123 Street', '0123456789', 'imageA.jpg', 'user1', 0, 0, 'passwordA'),
('Tran', 'Thi B', 1, '2023-02-01', '456 Street', '0987654321', 'imageB.jpg', 'user2', 1, 0, 'passwordB');

-- Insert data into SanPham
INSERT INTO SanPham (TenSP, DonViTinh, DonGia, Hinh)
VALUES
('Ca phe den', 'Ly', 20000, 'capheden.jpg'),
('Ca phe sua', 'Ly', 25000, 'caphesua.jpg'),
('Tra xanh', 'Ly', 15000, 'traxanh.jpg');

-- Insert data into HoaDon
INSERT INTO HoaDon (MaKH, NgayLapHD, NgayNhanHang, MaNV)
VALUES
('KH001', '2023-06-01', '2023-06-02', 1),
('KH002', '2023-06-03', '2023-06-04', 2),
('KH003', '2023-06-05', '2023-06-06', 3);

-- Insert data into ChiTietHoaDon
INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong)
VALUES
(1, 1, 2),
(1, 2, 1),
(2, 1, 1),
(2, 3, 3),
(3, 2, 2),
(3, 3, 1);