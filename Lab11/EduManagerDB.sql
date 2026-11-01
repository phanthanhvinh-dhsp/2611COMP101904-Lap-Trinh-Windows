USE master;
GO

-- Xóa database cũ nếu đã tồn tại để làm sạch hoàn toàn
IF DB_ID('EduManagerDB') IS NOT NULL
BEGIN
    ALTER DATABASE EduManagerDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EduManagerDB;
END
GO

-- Tạo mới lại database từ đầu
CREATE DATABASE EduManagerDB;
GO

USE EduManagerDB;
GO

-- Tạo lại các bảng theo đúng thứ tự (bảng cha LOP trước, bảng con SINHVIEN sau)
CREATE TABLE LOP (
    MaLop VARCHAR(20) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL
);

CREATE TABLE SINHVIEN (
    MaSV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    MaLop VARCHAR(20) NOT NULL,
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) REFERENCES LOP(MaLop)
);

-- Thêm dữ liệu mẫu
INSERT INTO LOP(MaLop, TenLop) VALUES
('CNTT01', N'Công nghệ thông tin 01'),
('CNTT02', N'Công nghệ thông tin 02'),
('SPTIN01', N'Sư phạm Tin học 01');

INSERT INTO SINHVIEN(MaSV, HoTen, MaLop) VALUES
('SV001', N'Nguyễn Văn An', 'CNTT01'),
('SV002', N'Trần Thị Bình', 'CNTT01');
GO
