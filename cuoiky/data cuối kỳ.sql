-- KHỞI TẠO DATABASE
CREATE DATABASE QuanLyNhanSu;
GO

USE QuanLyNhanSu;
GO

-- 1. Vai trò
CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL
);
GO

-- 2. Phòng ban
CREATE TABLE PhongBan (
    MaPhongBan INT PRIMARY KEY IDENTITY(1,1),
    TenPhongBan NVARCHAR(100) NOT NULL,
    TruongPhong INT NULL
);
GO

-- 3. Nhân viên
CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100),
    Phai NVARCHAR(10),
    NgaySinh DATE,
    SoDienThoai NVARCHAR(15),
    LuongCoBan DECIMAL(18,2),
    PhuCap DECIMAL(18,2),
    MaSoThue NVARCHAR(20),
    MaPhongBan INT,
    FOREIGN KEY (MaPhongBan) REFERENCES PhongBan(MaPhongBan)
);
GO

ALTER TABLE PhongBan
ADD CONSTRAINT FK_TruongPhong FOREIGN KEY (TruongPhong)
REFERENCES NhanVien(MaNV);
GO

-- 4. Dự án
CREATE TABLE DuAn (
    MaDuAn INT PRIMARY KEY IDENTITY(1,1),
    TenDuAn NVARCHAR(100),
    NgayBatDau DATE,
    NgayKetThuc DATE
);
GO

-- 5. Liên kết dự án - nhân viên
CREATE TABLE NhanVien_DuAn (
    MaNV INT,
    MaDuAn INT,
    NgayThamGia DATE,
    PRIMARY KEY (MaNV, MaDuAn),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn)
);
GO

-- 6. Tài khoản đăng nhập
CREATE TABLE NguoiDungDangNhap (
    TenDangNhap NVARCHAR(50) PRIMARY KEY,
    MatKhau NVARCHAR(255),
    MaNV INT,
    MaVaiTro INT,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO

-- 7. Bảng Lương (Lương hàng tháng)
CREATE TABLE Luong (
    MaLuong INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    Thang INT,
    Nam INT,
    LuongCoBan DECIMAL(18,2),
    PhuCap DECIMAL(18,2),
    Thuong DECIMAL(18,2),
    KhauTru DECIMAL(18,2),
    LuongThucLinh AS (LuongCoBan + PhuCap + Thuong - KhauTru),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- 8. Chi tiết dự án
CREATE TABLE ChiTietDuAn (
    MaChiTiet INT PRIMARY KEY IDENTITY(1,1),
    MaDuAn INT,
    NoiDung NVARCHAR(255),
    TrangThai NVARCHAR(50),
    NgayCapNhat DATE,
    FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn)
);
GO

-- 9. Chấm công
CREATE TABLE ChamCong (
    MaChamCong INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    NgayLam DATE,
    GioVao TIME,
    GioRa TIME,
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- 10. Hồ sơ nhân viên
CREATE TABLE HoSoNhanVien (
    MaHoSo INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    NgayTaoHoSo DATE,
    BangCap NVARCHAR(100),
    KinhNghiem NVARCHAR(255),
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO
-- Vai trò
INSERT INTO VaiTro (TenVaiTro) VALUES
(N'Nhân viên'),
(N'Trưởng phòng'),
(N'Nhân viên phòng nhân sự'),
(N'Trưởng phòng nhân sự'),
(N'Nhân viên phòng tài vụ'),
(N'Giám đốc');

-- Phòng ban
INSERT INTO PhongBan (TenPhongBan) VALUES
(N'Phòng Nhân Sự'),
(N'Phòng Tài Vụ'),
(N'Phòng Kinh Doanh');

-- Nhân viên
INSERT INTO NhanVien (HoTen, Phai, NgaySinh, SoDienThoai, LuongCoBan, PhuCap, MaSoThue, MaPhongBan)
VALUES
(N'Nguyễn Văn A', N'Nam', '1985-03-12', '0912345678', 20000000, 2000000, N'MST001', 1),
(N'Lê Thị B', N'Nữ', '1990-05-01', '0987654321', 15000000, 1500000, N'MST002', 1),
(N'Phạm Văn C', N'Nam', '1988-07-09', '0909876543', 18000000, 1000000, N'MST003', 2),
(N'Trần Thị D', N'Nữ', '1995-09-21', '0966666666', 12000000, 800000, N'MST004', 2),
(N'Hoàng Văn E', N'Nam', '1992-02-11', '0911111111', 17000000, 1200000, N'MST005', 3),
(N'Phan Thị F', N'Nữ', '1998-10-10', '0977777777', 10000000, 500000, N'MST006', 3),
(N'Lưu Quang G', N'Nam', '1975-01-20', '0900000000', 50000000, 5000000, N'MST007', 3);

-- Cập nhật trưởng phòng
UPDATE PhongBan SET TruongPhong = 1 WHERE MaPhongBan = 1;
UPDATE PhongBan SET TruongPhong = 3 WHERE MaPhongBan = 2;
UPDATE PhongBan SET TruongPhong = 5 WHERE MaPhongBan = 3;

-- Dự án
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc)
VALUES
(N'Hệ thống ERP nội bộ', '2025-01-01', '2025-12-31'),
(N'Phát triển website công ty', '2025-05-01', '2025-10-01'),
(N'Mở rộng chi nhánh miền Trung', '2025-02-15', '2026-02-15');
-- Liên kết nhân viên - dự án
INSERT INTO NhanVien_DuAn VALUES 
(1,1,'2025-01-05'), (2,1,'2025-02-01'), 
(3,2,'2025-05-05'), (5,2,'2025-06-01'), 
(6,3,'2025-03-01'), (7,3,'2025-03-10');

-- Tài khoản đăng nhập
INSERT INTO NguoiDungDangNhap (TenDangNhap, MatKhau, MaNV, MaVaiTro) VALUES
('nhansu_truong','123456',1,4),
('nhansu_nv','123456',2,3),
('taivu_truong','123456',3,2),
('taivu_nv','123456',4,5),
('kinhdoanh_tp','123456',5,2),
('kinhdoanh_nv','123456',6,1),
('giamdoc','123456',7,6);

-- Lương
INSERT INTO Luong (MaNV, Thang, Nam, LuongCoBan, PhuCap, Thuong, KhauTru)
VALUES
(1,4,2026,20000000,2000000,1000000,500000),
(2,4,2026,15000000,1500000,500000,300000),
(3,4,2026,18000000,1000000,200000,400000),
(4,4,2026,12000000,800000,0,200000),
(5,4,2026,17000000,1200000,700000,400000),
(6,4,2026,10000000,500000,0,100000),
(7,4,2026,50000000,5000000,5000000,0);

-- Chi tiết dự án
INSERT INTO ChiTietDuAn (MaDuAn, NoiDung, TrangThai, NgayCapNhat)
VALUES
(1,N'Xây dựng module nhân sự',N'Hoàn thành', '2025-08-01'),
(1,N'Phân hệ tài vụ',N'Đang thực hiện', '2026-03-20'),
(2,N'Thiết kế giao diện website',N'Hoàn thành','2025-08-10'),
(3,N'Trưng cầu ý kiến khách hàng',N'Chuẩn bị','2026-02-01');

-- Chấm công
INSERT INTO ChamCong (MaNV, NgayLam, GioVao, GioRa, GhiChu)
VALUES
(1,'2026-04-29','08:00','17:00',N'Làm bình thường'),
(2,'2026-04-29','08:10','17:05',N'Đến muộn'),
(3,'2026-04-29','08:00','16:50',N'Làm cả ngày'),
(6,'2026-04-29','09:00','18:00',N'Đi gặp khách hàng');

-- Hồ sơ nhân viên
INSERT INTO HoSoNhanVien (MaNV, NgayTaoHoSo, BangCap, KinhNghiem, GhiChu)
VALUES
(1,'2020-01-10',N'Cử nhân Quản trị nhân lực',N'10 năm kinh nghiệm HR',N'Trưởng phòng NS'),
(2,'2021-03-12',N'Cử nhân Kinh tế',N'4 năm kinh nghiệm nhân sự',NULL),
(5,'2019-09-09',N'Cử nhân Marketing',N'6 năm kinh nghiệm KD',NULL),
(7,'2010-01-01',N'Thạc sĩ Quản trị',N'Giám đốc điều hành công ty',NULL);
GO


-------Bổ sung dự án
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc)
VALUES 
(N'Nâng cấp hạ tầng mạng', '2026-01-01', '2026-06-01'),
(N'Đào tạo kỹ năng mềm nội bộ', '2026-02-10', '2026-03-10'),
(N'Số hóa hồ sơ nhân sự', '2026-01-15', '2026-12-31'),
(N'Phát triển App mobile', '2026-03-01', '2026-09-01'),
(N'Kiểm toán hệ thống bảo mật', '2026-04-01', '2026-05-01'),
(N'Khảo sát sự hài lòng nhân viên', '2026-05-15', '2026-06-15'),
(N'Xây dựng quy trình tuyển dụng mới', '2026-02-20', '2026-04-20'),
(N'Nghiên cứu thị trường miền Tây', '2026-06-01', '2026-11-01'),
(N'Tổ chức Team Building 2026', '2026-07-01', '2026-07-05'),
(N'Thiết kế bộ nhận diện thương hiệu', '2026-03-15', '2026-08-15');
-----
INSERT INTO NhanVien_DuAn (MaNV, MaDuAn, NgayThamGia)
SELECT 6, MaDuAn, GETDATE()
FROM (
    SELECT TOP 10 MaDuAn 
    FROM DuAn 
    ORDER BY MaDuAn DESC
) AS Projects;
-----------Bổ sung
-- 1. Thêm cột MaVaiTro vào bảng NhanVien
ALTER TABLE NhanVien ADD MaVaiTro INT;

-- 2. Tạo liên kết khóa ngoại
ALTER TABLE NhanVien ADD CONSTRAINT FK_NhanVien_VaiTro 
FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro);
-------------Bổ sung cột ảnh
-- Thêm cột để lưu tên file ảnh (ví dụ: nam.jpg)
ALTER TABLE NhanVien ADD HinhAnh NVARCHAR(255);
-- Gán tên file ảnh cho chính bạn
UPDATE NhanVien SET HinhAnh = 'nam_the.png' WHERE MaNV = 6;
UPDATE NhanVien SET HinhAnh = 'truongphong.png' WHERE MaNV = 1;
UPDATE NhanVien SET HinhAnh = 'Du.png' WHERE MaNV = 7;
-------- Bổ sung
-------- Bổ sung
CREATE TABLE LichSuHoatDong (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT,
    TenDangNhap NVARCHAR(50),
    HanhDong NVARCHAR(50),
    ChiTiet NVARCHAR(MAX),
    ThoiGian DATETIME DEFAULT GETDATE()
);