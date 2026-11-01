public class Nguoi
{
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }

    public Nguoi(string hoTen, DateTime ngaySinh)
    {
        HoTen = hoTen;
        NgaySinh = ngaySinh;
    }

    public virtual string LayThongTin() => $"{HoTen} - {NgaySinh:dd/MM/yyyy}";
}
