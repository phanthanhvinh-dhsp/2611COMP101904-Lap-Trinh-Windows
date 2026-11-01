public class FrmProduct : Form
{
    TextBox txtMa = new TextBox();
    TextBox txtTen = new TextBox();
    ComboBox cboLoai = new ComboBox();
    NumericUpDown numGia = new NumericUpDown { Maximum = 1000000000, ThousandsSeparator = true };
    NumericUpDown numSL = new NumericUpDown { Maximum = 100000 };
    TextBox txtTim = new TextBox();
    DataGridView dgv = new DataGridView();
    BindingSource src = new BindingSource();
    Button btnThem = new Button { Text = "Thêm" };
    Button btnSua = new Button { Text = "Sửa" };
    Button btnXoa = new Button { Text = "Xóa" };
    Button btnLamMoi = new Button { Text = "Làm mới" };
    Button btnTim = new Button { Text = "Tìm kiếm" };

    public FrmProduct()
    {
        Text = "Quản lý sản phẩm";
        ClientSize = new Size(760, 500);
        Add("Mã SP:", txtMa, 15, 15);
        Add("Tên SP:", txtTen, 15, 50);
        cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoai.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
        cboLoai.SelectedIndex = 0;
        Add("Loại SP:", cboLoai, 15, 85);
        Add("Đơn giá:", numGia, 380, 15);
        Add("Số lượng:", numSL, 380, 50);
        Add("Từ khóa:", txtTim, 380, 85);
        int x = 15;
        foreach (var b in new[] { btnThem, btnSua, btnXoa, btnLamMoi, btnTim })
        {
            b.SetBounds(x, 125, 100, 30);
            x += 110;
            Controls.Add(b);
        }
        dgv.SetBounds(15, 170, 730, 315);
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.MultiSelect = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        Controls.Add(dgv);
        src.DataSource = Data.Products;
        dgv.DataSource = src;

        btnThem.Click += BtnThem_Click;
        btnSua.Click += BtnSua_Click;
        btnXoa.Click += BtnXoa_Click;
        btnLamMoi.Click += (s, e) => { dgv.DataSource = src; Clear(); };
        btnTim.Click += BtnTim_Click;
        dgv.CellClick += (s, e) =>
        {
            if (e.RowIndex < 0 || src.Current is not Product p) return;
            txtMa.Text = p.MaSP; txtTen.Text = p.TenSP; cboLoai.Text = p.LoaiSP;
            numGia.Value = p.DonGia; numSL.Value = p.SoLuong;
        };
    }

    void Add(string text, Control c, int x, int y)
    {
        Controls.Add(new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true });
        c.Location = new Point(x + 80, y);
        c.Width = c is NumericUpDown ? 120 : 200;
        Controls.Add(c);
    }

    bool Check(bool isEdit)
    {
        string ma = txtMa.Text.Trim();
        if (ma.Length == 0 || string.IsNullOrWhiteSpace(txtTen.Text)) return Warn("Mã và tên sản phẩm không được rỗng.");
        if (!isEdit && Data.Products.Any(p => p.MaSP.Equals(ma, StringComparison.OrdinalIgnoreCase))) return Warn("Mã sản phẩm đã tồn tại.");
        if (numGia.Value < 0 || numSL.Value < 0) return Warn("Đơn giá và số lượng không được âm.");
        return true;
    }

    bool Warn(string msg)
    {
        MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    void BtnThem_Click(object sender, EventArgs e)
    {
        if (!Check(false)) return;
        Data.Products.Add(new Product { MaSP = txtMa.Text.Trim(), TenSP = txtTen.Text.Trim(), LoaiSP = cboLoai.Text, DonGia = numGia.Value, SoLuong = (int)numSL.Value });
        Clear();
    }

    void BtnSua_Click(object sender, EventArgs e)
    {
        if (src.Current is not Product p) { Warn("Vui lòng chọn một sản phẩm."); return; }
        txtMa.Text = p.MaSP;
        if (!Check(true)) return;
        p.TenSP = txtTen.Text.Trim(); p.LoaiSP = cboLoai.Text; p.DonGia = numGia.Value; p.SoLuong = (int)numSL.Value;
        src.ResetBindings(false);
    }

    void BtnXoa_Click(object sender, EventArgs e)
    {
        if (src.Current is not Product p) { Warn("Vui lòng chọn một sản phẩm."); return; }
        if (MessageBox.Show($"Xóa sản phẩm {p.TenSP}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Data.Products.Remove(p);
        Clear();
    }

    void BtnTim_Click(object sender, EventArgs e)
    {
        string kw = txtTim.Text.Trim().ToLower();
        dgv.DataSource = Data.Products.Where(p => p.MaSP.ToLower().Contains(kw) || p.TenSP.ToLower().Contains(kw)).ToList();
    }

    void Clear()
    {
        txtMa.Clear(); txtTen.Clear(); txtTim.Clear();
        cboLoai.SelectedIndex = 0; numGia.Value = 0; numSL.Value = 0;
        txtMa.Focus();
    }
}
