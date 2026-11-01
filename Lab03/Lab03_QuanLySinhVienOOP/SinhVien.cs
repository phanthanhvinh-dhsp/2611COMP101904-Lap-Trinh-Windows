public class SinhVien : Nguoi
{
    double diem;
    public string MaSinhVien { get; set; }
    public string MaLop { get; set; }

    public double DiemTrungBinh
    {
        get => diem;
        set
        {
            if (value < 0 || value > 10) throw new ArgumentOutOfRangeException(nameof(value), "Diem phai trong khoang 0-10");
            diem = value;
        }
    }

    public SinhVien(string ma, string hoTen, DateTime ngaySinh, string maLop, double diem) : base(hoTen, ngaySinh)
    {
        MaSinhVien = ma;
        MaLop = maLop;
        DiemTrungBinh = diem;
    }

    public string XepLoai() => diem >= 8 ? "Gioi" : diem >= 6.5 ? "Kha" : diem >= 5 ? "Trung binh" : "Yeu";

    public override string LayThongTin() => $"{MaSinhVien,-8} {HoTen,-22} {MaLop,-8} {diem,5:0.0}  {XepLoai()}";
}
