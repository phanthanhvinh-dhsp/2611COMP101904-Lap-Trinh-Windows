public class ProductService
{
    readonly Repository<Product> repo = new();

    public event Action<Product> ProductAdded;
    public event Action<Product> ProductRemoved;

    public void AddProduct(string ma, string ten, decimal price, int qty)
    {
        if (string.IsNullOrWhiteSpace(ma)) throw new ArgumentException("Ma san pham khong duoc rong");
        if (string.IsNullOrWhiteSpace(ten)) throw new ArgumentException("Ten san pham khong duoc rong");
        var p = new Product(ma.Trim(), ten.Trim(), price, qty);
        repo.Add(p);
        ProductAdded?.Invoke(p);
    }

    public void RemoveProduct(string ma)
    {
        var p = repo.FindById(ma) ?? throw new ProductNotFoundException(ma);
        repo.Remove(ma);
        ProductRemoved?.Invoke(p);
    }

    public Product FindById(string ma) => repo.FindById(ma);

    public List<Product> Search(string kw) => Filter(p => p.TenSP.Contains(kw, StringComparison.OrdinalIgnoreCase));

    public List<Product> Filter(Func<Product, bool> cond) => repo.Find(cond);

    public List<Product> GetAll() => repo.GetAll();

    public decimal TotalValue() => repo.GetAll().Sum(p => p.Price * p.Quantity);
}
