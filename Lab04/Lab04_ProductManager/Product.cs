public class Product : IEntity
{
    decimal price;
    int quantity;
    public string MaSP { get; }
    public string TenSP { get; set; }
    public string Id => MaSP;

    public decimal Price
    {
        get => price;
        set
        {
            if (value < 0) throw new ArgumentException("Don gia khong duoc am");
            price = value;
        }
    }

    public int Quantity
    {
        get => quantity;
        set
        {
            if (value < 0) throw new ArgumentException("So luong khong duoc am");
            quantity = value;
        }
    }

    public Product(string ma, string ten, decimal price, int quantity)
    {
        MaSP = ma;
        TenSP = ten;
        Price = price;
        Quantity = quantity;
    }

    public override string ToString() => $"{MaSP,-8} {TenSP,-20} {Price,12:N0} {Quantity,6}";
}
