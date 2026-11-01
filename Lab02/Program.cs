int[] a = null;

int ReadInt(string msg)
{
    while (true)
    {
        Console.Write(msg);
        if (int.TryParse(Console.ReadLine(), out int v)) return v;
        Console.WriteLine("Nhap sai, vui long nhap so nguyen.");
    }
}

int ReadPositive(string msg)
{
    while (true)
    {
        int v = ReadInt(msg);
        if (v > 0) return v;
        Console.WriteLine("Phai la so nguyen duong.");
    }
}

int[] Input()
{
    int n = ReadPositive("Nhap so phan tu n: ");
    var r = new int[n];
    for (int i = 0; i < n; i++) r[i] = ReadInt($"a[{i}] = ");
    return r;
}

void Print(int[] x) => Console.WriteLine(string.Join(" ", x));
int Sum(int[] x) { int s = 0; foreach (var v in x) s += v; return s; }
int Max(int[] x) { int m = x[0]; foreach (var v in x) if (v > m) m = v; return m; }
int Min(int[] x) { int m = x[0]; foreach (var v in x) if (v < m) m = v; return m; }
int CountEven(int[] x) { int c = 0; foreach (var v in x) if (v % 2 == 0) c++; return c; }
int CountOdd(int[] x) => x.Length - CountEven(x);

void SortAsc(int[] x)
{
    for (int i = 0; i < x.Length - 1; i++)
        for (int j = i + 1; j < x.Length; j++)
            if (x[i] > x[j]) (x[i], x[j]) = (x[j], x[i]);
}

int Find(int[] x, int k)
{
    for (int i = 0; i < x.Length; i++) if (x[i] == k) return i;
    return -1;
}

while (true)
{
    Console.WriteLine("\n===== MENU =====");
    Console.WriteLine("1. Nhap mang\n2. Xuat mang\n3. Tinh tong\n4. Tim max/min\n5. Dem chan/le\n6. Sap xep tang dan\n7. Tim kiem\n0. Thoat");
    int c = ReadInt("Chon chuc nang: ");
    if (c == 0) break;
    if (c < 0 || c > 7) { Console.WriteLine("Lua chon khong hop le."); continue; }
    if (c == 1) { a = Input(); continue; }
    if (a == null) { Console.WriteLine("Vui long nhap mang truoc (chon 1)."); continue; }
    switch (c)
    {
        case 2: Print(a); break;
        case 3: Console.WriteLine("Tong = " + Sum(a)); break;
        case 4: Console.WriteLine($"Max = {Max(a)}, Min = {Min(a)}"); break;
        case 5: Console.WriteLine($"Chan = {CountEven(a)}, Le = {CountOdd(a)}"); break;
        case 6: SortAsc(a); Print(a); break;
        case 7:
            int k = ReadInt("Nhap x: ");
            int p = Find(a, k);
            Console.WriteLine(p >= 0 ? $"Tim thay, vi tri dau tien: {p}" : "Khong tim thay.");
            break;
    }
}
