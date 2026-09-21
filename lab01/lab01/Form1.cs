using System;
using System.Windows.Forms;

namespace lab01;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // ===== Sự kiện Load: nạp dữ liệu cho ComboBox =====
    private void Form1_Load(object sender, EventArgs e)
    {
        cboKhoa.Items.Clear();
        cboKhoa.Items.AddRange(new object[]
        {
            "Công nghệ thông tin",
            "Quản trị kinh doanh",
            "Kế toán",
            "Ngôn ngữ Anh"
        });
        cboKhoa.SelectedIndex = -1; // chưa chọn gì lúc mở chương trình
    }

    // ===== Nút Hiển thị =====
    private void btnHienThi_Click(object sender, EventArgs e)
    {
        // Bước 1: kiểm tra dữ liệu, nếu sai thì dừng luôn
        if (!KiemTraDuLieu(out int namSinh))
            return;

        // Bước 2: tính tuổi và lấy giới tính
        int tuoi = DateTime.Now.Year - namSinh;
        string gioiTinh = radNam.Checked ? "Nam" : "Nữ";

        // Bước 3: hiển thị kết quả lên TextBox
        txtKetQua.Text =
            "THÔNG TIN SINH VIÊN" + Environment.NewLine +
            "Họ tên: " + txtHoTen.Text.Trim() + Environment.NewLine +
            "Tuổi: " + tuoi + Environment.NewLine +
            "Email: " + txtEmail.Text.Trim() + Environment.NewLine +
            "Giới tính: " + gioiTinh + Environment.NewLine +
            "Khoa/Lớp: " + cboKhoa.SelectedItem;
    }

    // ===== Nút Xóa =====
    private void btnXoa_Click(object sender, EventArgs e)
    {
        txtHoTen.Clear();
        txtNamSinh.Clear();
        txtEmail.Clear();
        txtKetQua.Clear();

        // Bỏ chọn giới tính
        radNam.Checked = false;
        radNu.Checked = false;

        // Đưa ComboBox về trạng thái không chọn
        cboKhoa.SelectedIndex = -1;

        txtHoTen.Focus(); // đặt con trỏ về ô đầu tiên
    }

    // ===== Nút Thoát =====
    private void btnThoat_Click(object sender, EventArgs e)
    {
        DialogResult kq = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát chương trình?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (kq == DialogResult.Yes)
            this.Close();
    }

    // ===== Hàm kiểm tra dữ liệu đầu vào =====
    // Trả về true nếu hợp lệ. namSinh trả ra giá trị năm sinh đã chuyển sang số nguyên.
    private bool KiemTraDuLieu(out int namSinh)
    {
        namSinh = 0;

        // 1. Họ tên không rỗng
        if (string.IsNullOrWhiteSpace(txtHoTen.Text))
        {
            BaoLoi("Họ tên không được để trống!", txtHoTen);
            return false;
        }

        // 2. Năm sinh không rỗng
        if (string.IsNullOrWhiteSpace(txtNamSinh.Text))
        {
            BaoLoi("Năm sinh không được để trống!", txtNamSinh);
            return false;
        }

        // 3. Năm sinh phải là số nguyên
        if (!int.TryParse(txtNamSinh.Text.Trim(), out namSinh))
        {
            BaoLoi("Năm sinh phải là số nguyên!", txtNamSinh);
            return false;
        }

        // 4. Năm sinh trong khoảng 1900 -> năm hiện tại
        int namHienTai = DateTime.Now.Year;
        if (namSinh < 1900 || namSinh > namHienTai)
        {
            BaoLoi($"Năm sinh phải nằm trong khoảng từ 1900 đến {namHienTai}!", txtNamSinh);
            return false;
        }

        // 5. Email không rỗng
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            BaoLoi("Email không được để trống!", txtEmail);
            return false;
        }

        // 6. Phải chọn giới tính
        if (!radNam.Checked && !radNu.Checked)
        {
            BaoLoi("Vui lòng chọn giới tính!", radNam);
            return false;
        }

        // 7. Phải chọn khoa/lớp
        if (cboKhoa.SelectedIndex < 0)
        {
            BaoLoi("Vui lòng chọn khoa hoặc lớp!", cboKhoa);
            return false;
        }

        return true;
    }

    // Hiện thông báo lỗi và đưa con trỏ về control bị lỗi
    private void BaoLoi(string thongBao, Control controlLoi)
    {
        MessageBox.Show(thongBao, "Dữ liệu không hợp lệ",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        controlLoi.Focus();
    }
}