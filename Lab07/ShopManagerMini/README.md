# Lab 07 - ShopManagerMini (SDI/MDI)

Chạy: mở `ShopManagerMini.csproj` bằng Visual Studio 2022+ (.NET 8), nhấn F5.
Tài khoản: `admin/123` (Admin), `user/123` (User).

## Câu hỏi nộp kèm

1. **SDI và MDI**: SDI mỗi cửa sổ là một Form độc lập, chỉ làm việc trên một tài liệu. MDI có một Form cha (`IsMdiContainer = true`) chứa nhiều Form con bên trong, dùng chung menu/status của cha.
2. **Vì sao login dùng ShowDialog()**: ShowDialog mở dạng modal, chặn chương trình cho đến khi đóng và trả `DialogResult`, nên Program biết đăng nhập thành công (OK) hay không trước khi mở FrmMain.
3. **Truyền user**: FrmLogin lưu user vào property `CurrentUser`; Program đọc nó rồi truyền vào constructor `new FrmMain(user)`.
4. **Không mở trùng Form**: `MdiChildren.OfType<T>().FirstOrDefault()`; nếu đã có thì `Activate()`, chưa có mới tạo mới với `MdiParent = this`.
5. **Constructor hay property**: constructor cho dữ liệu bắt buộc để Form hoạt động (như user của FrmMain); property cho dữ liệu tùy chọn hoặc cần lấy kết quả ngược lại sau khi Form đóng (như `CurrentUser`).
