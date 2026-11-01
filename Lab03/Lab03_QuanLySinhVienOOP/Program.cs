using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var ql = new QuanLySinhVien();

string ReadStr(string msg)
{
    while (true)
    {
        Console.Write(msg);
        var s = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(s)) return s;
        Console.WriteLine("Khong duoc de trong.");
    }
}

double ReadScore(string msg)
{
    while (true)
    {
        Console.Write(msg);
        if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double d) && d >= 0 && d <= 10) return d;
        Console.WriteLine("Diem khong hop le (0-10), nhap lai.");
    }
}

DateTime ReadDate(string msg)
{
    while (true)
    {
        Console.Write(msg);
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, DateTimeStyles.None, out var d)) return d;
        Console.WriteLine("Dinh dang dd/MM/yyyy, nhap lai.");
    }
}

void Show(List<SinhVien> l)
{
    if (l.Count == 0) { Console.WriteLine("Khong co du lieu."); return; }
    Console.WriteLine($"{"Ma",-8} {"Ho ten",-22} {"Lop",-8} {"Diem",5}  Xep loai");
    foreach (var s in l) Console.WriteLine(s.LayThongTin());
}

ql.Them(new SinhVien("SV001", "Nguyễn Văn A", new DateTime(2005, 1, 15), "CTK47A", 8.2));
ql.Them(new SinhVien("SV002", "Trần Thị B", new DateTime(2005, 5, 20), "CTK47A", 4.5));

while (true)
{
    Console.WriteLine("\n===== QUAN LY SINH VIEN =====");
    Console.WriteLine("1. Them sinh vien\n2. Xuat danh sach\n3. Tim sinh vien theo ma\n4. Tim sinh vien theo ten\n5. Sua diem trung binh\n6. Xoa sinh vien\n7. Sap xep theo diem giam dan\n8. Loc sinh vien dat\n0. Thoat");
    Console.Write("Chon chuc nang: ");
    if (!int.TryParse(Console.ReadLine(), out int c)) { Console.WriteLine("Vui long nhap so."); continue; }
    if (c == 0) break;
    switch (c)
    {
        case 1:
            var ma = ReadStr("Ma SV: ");
            if (ql.TimTheoMa(ma) != null) { Console.WriteLine("Ma sinh vien da ton tai."); break; }
            var ten = ReadStr("Ho ten: ");
            var ns = ReadDate("Ngay sinh (dd/MM/yyyy): ");
            var lop = ReadStr("Ma lop: ");
            var diem = ReadScore("Diem TB: ");
            ql.Them(new SinhVien(ma, ten, ns, lop, diem));
            Console.WriteLine("Them thanh cong.");
            break;
        case 2: Show(ql.LayDanhSach()); break;
        case 3:
            var f = ql.TimTheoMa(ReadStr("Ma SV: "));
            if (f == null) Console.WriteLine("Khong tim thay."); else Show(new List<SinhVien> { f });
            break;
        case 4: Show(ql.TimTheoTen(ReadStr("Tu khoa: "))); break;
        case 5:
            var m5 = ReadStr("Ma SV: ");
            if (ql.TimTheoMa(m5) == null) { Console.WriteLine("Khong tim thay."); break; }
            ql.Sua(m5, ReadScore("Diem moi: "));
            Console.WriteLine("Da cap nhat.");
            break;
        case 6: Console.WriteLine(ql.Xoa(ReadStr("Ma SV: ")) ? "Da xoa." : "Khong tim thay."); break;
        case 7: Show(ql.SapXepTheoDiem()); break;
        case 8: Show(ql.LocDat()); break;
        default: Console.WriteLine("Lua chon khong hop le."); break;
    }
}
