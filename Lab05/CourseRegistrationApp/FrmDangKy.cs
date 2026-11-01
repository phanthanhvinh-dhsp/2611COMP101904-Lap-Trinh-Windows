public class FrmDangKy : Form
{
    TextBox txtHoTen = new TextBox();
    TextBox txtSoDienThoai = new TextBox();
    DateTimePicker dtpNgaySinh = new DateTimePicker();
    CheckBox chkNhanEmail = new CheckBox();
    ComboBox cboKhoaHoc = new ComboBox();
    RadioButton radOnline = new RadioButton();
    RadioButton radOffline = new RadioButton();
    NumericUpDown numSoThang = new NumericUpDown();
    Label lblTongTien = new Label();
    Button btnDangKy = new Button();
    Button btnLamMoi = new Button();
    Button btnThoat = new Button();

    readonly Dictionary<string, int> hocPhi = new Dictionary<string, int>
    {
        { "C# WinForms cơ bản", 800000 },
        { "SQL Server cơ bản", 700000 },
        { "Web Frontend cơ bản", 750000 },
        { "Lập trình Python cơ bản", 650000 }
    };

    public FrmDangKy()
    {
        Text = "ĐĂNG KÝ KHÓA HỌC";
        ClientSize = new Size(480, 500);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var gbHocVien = new GroupBox { Text = "Thông tin học viên", Location = new Point(15, 15), Size = new Size(450, 170) };
        Row(gbHocVien, "Họ tên:", txtHoTen, 30);
        Row(gbHocVien, "Số điện thoại:", txtSoDienThoai, 65);
        dtpNgaySinh.Format = DateTimePickerFormat.Short;
        Row(gbHocVien, "Ngày sinh:", dtpNgaySinh, 100);
        chkNhanEmail.Text = "Nhận email thông báo";
        chkNhanEmail.AutoSize = true;
        chkNhanEmail.Location = new Point(130, 135);
        gbHocVien.Controls.Add(chkNhanEmail);

        var gbKhoaHoc = new GroupBox { Text = "Thông tin khóa học", Location = new Point(15, 200), Size = new Size(450, 200) };
        cboKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
        Row(gbKhoaHoc, "Khóa học:", cboKhoaHoc, 30);
        radOnline.Text = "Online"; radOnline.AutoSize = true; radOnline.Location = new Point(130, 70);
        radOffline.Text = "Trực tiếp"; radOffline.AutoSize = true; radOffline.Location = new Point(230, 70);
        gbKhoaHoc.Controls.AddRange(new Control[] { new Label { Text = "Hình thức:", Location = new Point(15, 72), AutoSize = true }, radOnline, radOffline });
        numSoThang.Minimum = 1;
        numSoThang.Maximum = 12;
        Row(gbKhoaHoc, "Số tháng:", numSoThang, 105);
        gbKhoaHoc.Controls.Add(new Label { Text = "Tổng học phí:", Location = new Point(15, 155), AutoSize = true });
        lblTongTien.Location = new Point(130, 152);
        lblTongTien.AutoSize = true;
        lblTongTien.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        lblTongTien.ForeColor = Color.Firebrick;
        gbKhoaHoc.Controls.Add(lblTongTien);

        btnDangKy.Text = "Đăng ký"; btnDangKy.Location = new Point(60, 425); btnDangKy.Size = new Size(110, 36);
        btnLamMoi.Text = "Làm mới"; btnLamMoi.Location = new Point(185, 425); btnLamMoi.Size = new Size(110, 36);
        btnThoat.Text = "Thoát"; btnThoat.Location = new Point(310, 425); btnThoat.Size = new Size(110, 36);

        Controls.AddRange(new Control[] { gbHocVien, gbKhoaHoc, btnDangKy, btnLamMoi, btnThoat });

        Load += FrmDangKy_Load;
        cboKhoaHoc.SelectedIndexChanged += (s, e) => CapNhatTien();
        numSoThang.ValueChanged += (s, e) => CapNhatTien();
        btnDangKy.Click += BtnDangKy_Click;
        btnLamMoi.Click += (s, e) => LamMoi();
        btnThoat.Click += BtnThoat_Click;

        txtHoTen.TabIndex = 0;
        txtSoDienThoai.TabIndex = 1;
        dtpNgaySinh.TabIndex = 2;
        chkNhanEmail.TabIndex = 3;
        gbHocVien.TabIndex = 0;
        gbKhoaHoc.TabIndex = 1;
        btnDangKy.TabIndex = 2;
        btnLamMoi.TabIndex = 3;
        btnThoat.TabIndex = 4;
    }

    void Row(GroupBox gb, string text, Control c, int y)
    {
        gb.Controls.Add(new Label { Text = text, Location = new Point(15, y + 3), AutoSize = true });
        c.Location = new Point(130, y);
        c.Width = c is NumericUpDown ? 80 : 290;
        gb.Controls.Add(c);
    }

    void FrmDangKy_Load(object sender, EventArgs e)
    {
        cboKhoaHoc.Items.AddRange(hocPhi.Keys.ToArray());
        LamMoi();
    }

    int Gia() => cboKhoaHoc.SelectedIndex < 0 ? 0 : hocPhi[cboKhoaHoc.Text];

    void CapNhatTien()
    {
        lblTongTien.Text = (Gia() * (int)numSoThang.Value).ToString("N0") + " VNĐ";
    }

    void BtnDangKy_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtHoTen.Text))
        {
            MessageBox.Show("Họ tên không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtHoTen.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
        {
            MessageBox.Show("Số điện thoại không được rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSoDienThoai.Focus();
            return;
        }
        if (cboKhoaHoc.SelectedIndex < 0)
        {
            MessageBox.Show("Vui lòng chọn khóa học.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboKhoaHoc.Focus();
            return;
        }
        int tong = Gia() * (int)numSoThang.Value;
        string msg = "PHIẾU ĐĂNG KÝ KHÓA HỌC\n\n" +
                     $"Họ tên: {txtHoTen.Text.Trim()}\n" +
                     $"Số điện thoại: {txtSoDienThoai.Text.Trim()}\n" +
                     $"Ngày sinh: {dtpNgaySinh.Value:dd/MM/yyyy}\n" +
                     $"Khóa học: {cboKhoaHoc.Text}\n" +
                     $"Hình thức: {(radOnline.Checked ? "Online" : "Trực tiếp")}\n" +
                     $"Số tháng: {numSoThang.Value}\n" +
                     $"Tổng tiền: {tong:N0} VNĐ\n" +
                     $"Nhận email: {(chkNhanEmail.Checked ? "Có" : "Không")}";
        MessageBox.Show(msg, "Đăng ký thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    void LamMoi()
    {
        txtHoTen.Clear();
        txtSoDienThoai.Clear();
        dtpNgaySinh.Value = DateTime.Today;
        chkNhanEmail.Checked = false;
        if (cboKhoaHoc.Items.Count > 0) cboKhoaHoc.SelectedIndex = 0;
        radOnline.Checked = true;
        numSoThang.Value = 1;
        CapNhatTien();
        txtHoTen.Focus();
    }

    void BtnThoat_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
    }
}
