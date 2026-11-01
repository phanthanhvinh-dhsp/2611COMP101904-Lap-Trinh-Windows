USE EduManagerDB;
GO

-- 1
SELECT MaSV, HoTen, NgaySinh, GioiTinh, MaLop FROM SINHVIEN;

-- 2
SELECT sv.MaSV, sv.HoTen, l.TenLop, k.TenKhoa
FROM SINHVIEN sv
INNER JOIN LOP l ON sv.MaLop = l.MaLop
INNER JOIN KHOA k ON l.MaKhoa = k.MaKhoa;

-- 3
SELECT MaSV, HoTen, MaLop FROM SINHVIEN WHERE HoTen LIKE N'%An%' ORDER BY HoTen;

-- 4
SELECT MaSV, HoTen, GioiTinh FROM SINHVIEN WHERE MaLop = 'CNTT01';

-- 5
UPDATE SINHVIEN SET HoTen = N'Nguyễn Văn An Updated' WHERE MaSV = 'SV001';
SELECT MaSV, HoTen FROM SINHVIEN WHERE MaSV = 'SV001';

-- 6
DELETE FROM SINHVIEN
WHERE MaSV = 'SV003' AND NOT EXISTS (SELECT 1 FROM KETQUA WHERE MaSV = 'SV003');
DELETE FROM SINHVIEN WHERE MaSV = 'SV006' AND NOT EXISTS (SELECT 1 FROM KETQUA WHERE MaSV = 'SV006');

-- 7
SELECT sv.MaSV, sv.HoTen, mh.TenMH, kq.Diem
FROM KETQUA kq
INNER JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV
INNER JOIN MONHOC mh ON kq.MaMH = mh.MaMH
ORDER BY sv.MaSV, mh.TenMH;

-- 8
SELECT sv.MaSV, sv.HoTen, AVG(kq.Diem) AS DiemTrungBinh
FROM SINHVIEN sv
INNER JOIN KETQUA kq ON sv.MaSV = kq.MaSV
GROUP BY sv.MaSV, sv.HoTen;

-- 9
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop;

-- 10
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
HAVING COUNT(sv.MaSV) >= 2;

-- 11 loi: lop CNTT99 khong ton tai
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)
VALUES ('SV999', N'Sinh viên lỗi', '2005-01-01', N'Nam', 'CNTT99');

-- 12 loi: diem ngoai 0..10
INSERT INTO KETQUA (MaSV, MaMH, Diem) VALUES ('SV002', 'COMP1017', 12);

-- 13 loi: gioi tinh khong hop le
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)
VALUES ('SV998', N'Giới tính lỗi', '2005-01-01', N'Khác', 'CNTT01');

-- 14 loi: trung khoa chinh
INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, MaLop)
VALUES ('SV001', N'Trùng mã', '2005-01-01', N'Nam', 'CNTT01');
GO
