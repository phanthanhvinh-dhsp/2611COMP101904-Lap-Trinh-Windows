using System.ComponentModel;

public static class Data
{
    public static List<User> Users = new List<User>
    {
        new User { Username = "admin", Password = "123", HoTen = "Quản trị viên", VaiTro = "Admin" },
        new User { Username = "user", Password = "123", HoTen = "Nhân viên bán hàng", VaiTro = "User" }
    };

    public static BindingList<Product> Products = new BindingList<Product>
    {
        new Product { MaSP = "SP01", TenSP = "Chuột không dây", LoaiSP = "Điện tử", DonGia = 150000, SoLuong = 20 },
        new Product { MaSP = "SP02", TenSP = "Bút bi", LoaiSP = "Văn phòng", DonGia = 5000, SoLuong = 100 }
    };

    public static BindingList<Customer> Customers = new BindingList<Customer>
    {
        new Customer { MaKH = "KH01", HoTen = "Nguyễn Văn An", SoDienThoai = "0901234567", LoaiKH = "Thường" }
    };
}
