public class QuanLySinhVien
{
    readonly List<SinhVien> ds = new();

    public bool Them(SinhVien sv)
    {
        if (TimTheoMa(sv.MaSinhVien) != null) return false;
        ds.Add(sv);
        return true;
    }

    public bool Sua(string ma, double diem)
    {
        var sv = TimTheoMa(ma);
        if (sv == null) return false;
        sv.DiemTrungBinh = diem;
        return true;
    }

    public bool Xoa(string ma)
    {
        var sv = TimTheoMa(ma);
        return sv != null && ds.Remove(sv);
    }

    public SinhVien TimTheoMa(string ma) => ds.FirstOrDefault(s => s.MaSinhVien.Equals(ma, StringComparison.OrdinalIgnoreCase));

    public List<SinhVien> TimTheoTen(string kw) => ds.Where(s => s.HoTen.Contains(kw, StringComparison.OrdinalIgnoreCase)).ToList();

    public List<SinhVien> SapXepTheoDiem() => ds.OrderByDescending(s => s.DiemTrungBinh).ToList();

    public List<SinhVien> LocDat() => ds.Where(s => s.DiemTrungBinh >= 5).ToList();

    public List<SinhVien> LayDanhSach() => ds.ToList();
}
