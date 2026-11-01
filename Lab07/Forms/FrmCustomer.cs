public class FrmCustomer : Form
{
    TextBox txtMa = new TextBox();
    TextBox txtTen = new TextBox();
    TextBox txtSdt = new TextBox();
    ComboBox cboLoai = new ComboBox();
    DataGridView dgv = new DataGridView();
    Button btnThem = new Button { Text = "Thêm" };
    Button btnXoa = new Button { Text = "Xóa" };

    public FrmCustomer()
    {
        Text = "Quản lý khách hàng";
        ClientSize = new Size(700, 440);
        Add("Mã KH:", txtMa, 15, 15);
        Add("Họ tên:", txtTen, 15, 50);
        Add("SĐT:", txtSdt, 350, 15);
        cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoai.Items.AddRange(new object[] { "Thường", "VIP" });
        cboLoai.SelectedIndex = 0;
        Add("Loại KH:", cboLoai, 350, 50);
        btnThem.SetBounds(15, 90, 100, 30);
        btnXoa.SetBounds(125, 90, 100, 30);
        Controls.AddRange(new Control[] { btnThem, btnXoa });
        dgv.SetBounds(15, 135, 670, 290);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.DataSource = Data.Customers;
        Controls.Add(dgv);
        btnThem.Click += BtnThem_Click;
        btnXoa.Click += BtnXoa_Click;
    }

    void Add(string text, Control c, int x, int y)
    {
        Controls.Add(new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true });
        c.Location = new Point(x + 70, y);
        c.Width = 200;
        Controls.Add(c);
    }

    void BtnThem_Click(object sender, EventArgs e)
    {
        string ma = txtMa.Text.Trim();
        if (ma.Length == 0 || string.IsNullOrWhiteSpace(txtTen.Text))
        {
            MessageBox.Show("Mã và họ tên không được rỗng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (Data.Customers.Any(c => c.MaKH.Equals(ma, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã khách hàng đã tồn tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Data.Customers.Add(new Customer { MaKH = ma, HoTen = txtTen.Text.Trim(), SoDienThoai = txtSdt.Text.Trim(), LoaiKH = cboLoai.Text });
        txtMa.Clear(); txtTen.Clear(); txtSdt.Clear();
        txtMa.Focus();
    }

    void BtnXoa_Click(object sender, EventArgs e)
    {
        if (dgv.CurrentRow?.DataBoundItem is not Customer c) return;
        if (MessageBox.Show($"Xóa khách hàng {c.HoTen}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Data.Customers.Remove(c);
    }
}
