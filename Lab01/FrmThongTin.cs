public class FrmThongTin : Form
{
    Label lblTitle = new Label();
    TextBox txtHoTen = new TextBox();
    TextBox txtNamSinh = new TextBox();
    TextBox txtEmail = new TextBox();
    RadioButton radNam = new RadioButton();
    RadioButton radNu = new RadioButton();
    ComboBox cboKhoa = new ComboBox();
    Button btnHienThi = new Button();
    Button btnXoa = new Button();
    Button btnThoat = new Button();
    TextBox txtKetQua = new TextBox();

    public FrmThongTin()
    {
        Text = "Ứng dụng thông tin cá nhân";
        ClientSize = new Size(460, 470);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        lblTitle.Text = "THÔNG TIN SINH VIÊN";
        lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        lblTitle.ForeColor = Color.Navy;
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(110, 15);

        AddRow("Họ tên:", txtHoTen, 70);
        AddRow("Năm sinh:", txtNamSinh, 105);
        AddRow("Email:", txtEmail, 140);

        var lblGt = new Label { Text = "Giới tính:", Location = new Point(25, 180), AutoSize = true };
        var grp = new GroupBox { Text = "", Location = new Point(130, 165), Size = new Size(280, 40) };
        radNam.Text = "Nam"; radNam.Location = new Point(15, 13); radNam.AutoSize = true;
        radNu.Text = "Nữ"; radNu.Location = new Point(110, 13); radNu.AutoSize = true;
        grp.Controls.AddRange(new Control[] { radNam, radNu });

        cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKhoa.Items.AddRange(new object[] { "Công nghệ thông tin", "Toán - Tin học", "Sư phạm Tin học", "Vật lý" });
        cboKhoa.SelectedIndex = 0;
        AddRow("Khoa/Lớp:", cboKhoa, 220);

        btnHienThi.Text = "Hiển thị"; btnHienThi.Location = new Point(40, 265); btnHienThi.Size = new Size(110, 32);
        btnXoa.Text = "Xóa"; btnXoa.Location = new Point(175, 265); btnXoa.Size = new Size(110, 32);
        btnThoat.Text = "Thoát"; btnThoat.Location = new Point(310, 265); btnThoat.Size = new Size(110, 32);

        txtKetQua.Multiline = true;
        txtKetQua.ReadOnly = true;
        txtKetQua.Location = new Point(25, 315);
        txtKetQua.Size = new Size(410, 135);
        txtKetQua.BackColor = Color.White;

        Controls.AddRange(new Control[] { lblTitle, lblGt, grp, btnHienThi, btnXoa, btnThoat, txtKetQua });

        btnHienThi.Click += BtnHienThi_Click;
        btnXoa.Click += BtnXoa_Click;
        btnThoat.Click += BtnThoat_Click;
        AcceptButton = btnHienThi;
    }

    void AddRow(string text, Control c, int y)
    {
        Controls.Add(new Label { Text = text, Location = new Point(25, y + 3), AutoSize = true });
        c.Location = new Point(130, y);
        c.Width = 280;
        Controls.Add(c);
    }

    bool Validate2(out int year)
    {
        year = 0;
        if (string.IsNullOrWhiteSpace(txtHoTen.Text)) return Fail("Họ tên không được rỗng.", txtHoTen);
        if (string.IsNullOrWhiteSpace(txtNamSinh.Text)) return Fail("Năm sinh không được rỗng.", txtNamSinh);
        if (!int.TryParse(txtNamSinh.Text.Trim(), out year)) return Fail("Năm sinh phải là số nguyên.", txtNamSinh);
        if (year < 1900 || year > DateTime.Now.Year) return Fail($"Năm sinh phải từ 1900 đến {DateTime.Now.Year}.", txtNamSinh);
        if (string.IsNullOrWhiteSpace(txtEmail.Text)) return Fail("Email không được rỗng.", txtEmail);
        if (!radNam.Checked && !radNu.Checked) return Fail("Vui lòng chọn giới tính.", radNam);
        if (cboKhoa.SelectedIndex < 0) return Fail("Vui lòng chọn khoa/lớp.", cboKhoa);
        return true;
    }

    bool Fail(string msg, Control c)
    {
        MessageBox.Show(msg, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        c.Focus();
        return false;
    }

    void BtnHienThi_Click(object sender, EventArgs e)
    {
        if (!Validate2(out int year)) return;
        string gt = radNam.Checked ? "Nam" : "Nữ";
        string kq = "THÔNG TIN SINH VIÊN" + Environment.NewLine +
                    $"Họ tên: {txtHoTen.Text.Trim()}" + Environment.NewLine +
                    $"Tuổi: {DateTime.Now.Year - year}" + Environment.NewLine +
                    $"Email: {txtEmail.Text.Trim()}" + Environment.NewLine +
                    $"Giới tính: {gt}" + Environment.NewLine +
                    $"Khoa/Lớp: {cboKhoa.Text}";
        txtKetQua.Text = kq;
        MessageBox.Show(kq, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    void BtnXoa_Click(object sender, EventArgs e)
    {
        txtHoTen.Clear();
        txtNamSinh.Clear();
        txtEmail.Clear();
        txtKetQua.Clear();
        radNam.Checked = false;
        radNu.Checked = false;
        cboKhoa.SelectedIndex = 0;
        txtHoTen.Focus();
    }

    void BtnThoat_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
    }
}
