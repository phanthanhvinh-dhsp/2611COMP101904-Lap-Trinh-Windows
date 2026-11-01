public class DuplicateProductException : Exception
{
    public DuplicateProductException(string id) : base($"Ma san pham '{id}' da ton tai.") { }
}

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string id) : base($"Khong tim thay san pham '{id}'.") { }
}
