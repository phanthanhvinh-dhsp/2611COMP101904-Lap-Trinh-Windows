# Lab 11 - ADO.NET CRUD (FrmLop)

1. Chạy `EduManagerDB.sql` trong SSMS.
2. Mở `COMP1019_Buoi11_Lab11.csproj` bằng Visual Studio 2022+ (.NET 8), F5 (tự restore Microsoft.Data.SqlClient).

Connection string (trong `FrmLop.cs`):

    Data Source=.;Initial Catalog=EduManagerDB;Integrated Security=True;TrustServerCertificate=True

Nếu dùng SQL Express đổi thành `Data Source=.\SQLEXPRESS`.
Click một dòng sẽ khóa ô Mã lớp (khóa chính); bấm Làm mới để mở khóa và nhập lớp mới.
