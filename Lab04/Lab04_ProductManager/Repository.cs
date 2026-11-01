public class Repository<T> where T : IEntity
{
    readonly List<T> items = new();

    public void Add(T item)
    {
        if (FindById(item.Id) != null) throw new DuplicateProductException(item.Id);
        items.Add(item);
    }

    public void Remove(string id)
    {
        var item = FindById(id) ?? throw new ProductNotFoundException(id);
        items.Remove(item);
    }

    public T FindById(string id) => items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

    public List<T> Find(Func<T, bool> predicate) => items.Where(predicate).ToList();

    public List<T> GetAll() => items.ToList();
}
