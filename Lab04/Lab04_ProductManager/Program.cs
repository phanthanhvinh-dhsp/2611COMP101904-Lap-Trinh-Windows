using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var sv = new ProductService();
sv.ProductAdded += p => Console.WriteLine($"[Event] Da them san pham {p.MaSP}");
sv.ProductRemoved += p => Console.WriteLine($"[Event] Da xoa san pham {p.MaSP}");

string ReadStr(string msg)
{
    Console.Write(msg);
    return Console.ReadLine()?.Trim() ?? "";
}

decimal ReadDecimal(string msg)
{
    while (true)
    {
        if (decimal.TryParse(ReadStr(msg), out var d)) return d;
        Console.WriteLine("Nhap sai, vui long nhap so.");
    }
}

int ReadInt(string msg)
{
    while (true)
    {
        if (int.TryParse(ReadStr(msg), out var d)) return d;
        Console.WriteLine("Nhap sai, vui long nhap so nguyen.");
    }
}

void Show(List<Product> l)
{
    if (l.Count == 0) { Console.WriteLine("Khong co san pham nao."); return; }
    Console.WriteLine($"{"Ma",-8} {"Ten",-20} {"Don gia",12} {"SL",6}");
    l.ForEach(Console.WriteLine);
}

while (true)
{
    Console.WriteLine("\n===== PRODUCT MANAGER =====");
    Console.WriteLine("1. Them san pham\n2. Xuat danh sach\n3. Tim theo ma\n4. Tim theo ten\n5. Loc theo khoang gia\n6. Xoa san pham\n7. Tinh tong gia tri kho\n0. Thoat");
    int c = ReadInt("Chon: ");
    if (c == 0) break;
    try
    {
        switch (c)
        {
            case 1: sv.AddProduct(ReadStr("Ma: "), ReadStr("Ten: "), ReadDecimal("Don gia: "), ReadInt("So luong: ")); break;
            case 2: Show(sv.GetAll()); break;
            case 3:
                var p = sv.FindById(ReadStr("Ma: "));
                if (p == null) Console.WriteLine("Khong tim thay."); else Show(new List<Product> { p });
                break;
            case 4: Show(sv.Search(ReadStr("Tu khoa: "))); break;
            case 5:
                decimal lo = ReadDecimal("Gia nho nhat: "), hi = ReadDecimal("Gia lon nhat: ");
                Show(sv.Filter(x => x.Price >= lo && x.Price <= hi));
                break;
            case 6: sv.RemoveProduct(ReadStr("Ma: ")); break;
            case 7: Console.WriteLine($"Tong gia tri kho: {sv.TotalValue():N0}"); break;
            default: Console.WriteLine("Lua chon khong hop le."); break;
        }
    }
    catch (DuplicateProductException ex) { Console.WriteLine("Loi: " + ex.Message); }
    catch (ProductNotFoundException ex) { Console.WriteLine("Loi: " + ex.Message); }
    catch (ArgumentException ex) { Console.WriteLine("Du lieu khong hop le: " + ex.Message); }
}
