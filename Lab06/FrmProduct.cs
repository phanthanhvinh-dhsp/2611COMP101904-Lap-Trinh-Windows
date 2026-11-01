public class FrmProduct : Form
{
    BindingList<Product> products = new BindingList<Product>();
    BindingSource productSource = new BindingSource();
    string selectedImagePath = "";

    MenuStrip menu = new MenuStrip();
    ToolStrip toolbar = new ToolStrip();
    StatusStrip status = new StatusStrip();
    ToolStripStatusLabel lblStatus = new ToolStripStatusLabel();
    ContextMenuStrip ctx = new ContextMenuStrip();
    ErrorProvider errorProvider1 = new ErrorProvider();

    TextBox txtMaSP = new TextBox();
    TextBox txtTenSP = new TextBox();
    ComboBox cboLoaiSP = new ComboBox();
    NumericUpDown numDonGia = new NumericUpDown();
    NumericUpDown numSoLuong = new NumericUpDown();
    DateTimePicker dtpNgayNhap = new DateTimePicker();
    CheckBox chkConKinhDoanh = new CheckBox();
    PictureBox picSanPham = new PictureBox();
    TextBox txtTimKiem = new TextBox();
    DataGridView dgvProducts = new DataGridView();
    Button btnThem = new Button();
    Button btnSua = new Button();
    Button btnXoa = new Button();
    Button btnLamMoi = new Button();
    Button btnTimKiem = new Button();
    Button btnChonAnh = new Button();

    public FrmProduct()
    {
        Text = "Quản lý sản phẩm";
        ClientSize = new Size(960, 640);
        StartPosition = FormStartPosition.CenterScreen;
        BuildUI();
        Load += FrmProduct_Load;
    }

    void BuildUI()
    {
        var mFile = new ToolStripMenuItem("File");
        var mExit = new ToolStripMenuItem("Thoát", null, (s, e) => Close()) { ShortcutKeys = Keys.Alt | Keys.F4 };
        mFile.DropDownItems.Add(mExit);
        var mQl = new ToolStripMenuItem("Quản lý");
        mQl.DropDownItems.Add(new ToolStripMenuItem("Thêm", null, (s, e) => btnThem.PerformClick()) { ShortcutKeys = Keys.Control | Keys.N });
        mQl.DropDownItems.Add(new ToolStripMenuItem("Sửa", null, (s, e) => btnSua.PerformClick()) { ShortcutKeys = Keys.Control | Keys.E });
        mQl.DropDownItems.Add(new ToolStripMenuItem("Xóa", null, (s, e) => btnXoa.PerformClick()) { ShortcutKeys = Keys.Control | Keys.D });
        var mHelp = new ToolStripMenuItem("Trợ giúp");
        mHelp.DropDownItems.Add(new ToolStripMenuItem("Giới thiệu", null, (s, e) => MessageBox.Show("ProductManager - COMP1019 Lab 05", "Giới thiệu")));
        menu.Items.AddRange(new ToolStripItem[] { mFile, mQl, mHelp });

        toolbar.Items.Add(new ToolStripButton("Thêm", null, (s, e) => btnThem.PerformClick()));
        toolbar.Items.Add(new ToolStripButton("Sửa", null, (s, e) => btnSua.PerformClick()));
        toolbar.Items.Add(new ToolStripButton("Xóa", null, (s, e) => btnXoa.PerformClick()));
        toolbar.Items.Add(new ToolStripButton("Làm mới", null, (s, e) => btnLamMoi.PerformClick()));
        toolbar.Items.Add(new ToolStripButton("Tìm kiếm", null, (s, e) => btnTimKiem.PerformClick()));

        status.Items.Add(lblStatus);

        ctx.Items.Add(new ToolStripMenuItem("Sửa", null, (s, e) => btnSua.PerformClick()));
        ctx.Items.Add(new ToolStripMenuItem("Xóa", null, (s, e) => btnXoa.PerformClick()));
        ctx.Items.Add(new ToolStripMenuItem("Xem chi tiết", null, CmsChiTiet_Click));

        var gb = new GroupBox { Text = "Thông tin sản phẩm", Location = new Point(10, 55), Size = new Size(940, 215) };
        AddField(gb, "Mã SP:", txtMaSP, 15, 30);
        AddField(gb, "Tên SP:", txtTenSP, 15, 65);
        cboLoaiSP.DropDownStyle = ComboBoxStyle.DropDownList;
        AddField(gb, "Loại SP:", cboLoaiSP, 15, 100);
        numDonGia.Maximum = 1000000000; numDonGia.Minimum = 0; numDonGia.ThousandsSeparator = true;
        AddField(gb, "Đơn giá:", numDonGia, 15, 135);
        numSoLuong.Maximum = 100000;
        AddField(gb, "Số lượng:", numSoLuong, 330, 30);
        dtpNgayNhap.Format = DateTimePickerFormat.Short;
        AddField(gb, "Ngày nhập:", dtpNgayNhap, 330, 65);
        chkConKinhDoanh.Text = "Còn kinh doanh"; chkConKinhDoanh.AutoSize = true; chkConKinhDoanh.Checked = true;
        chkConKinhDoanh.Location = new Point(410, 102);
        gb.Controls.Add(chkConKinhDoanh);
        AddField(gb, "Tìm kiếm:", txtTimKiem, 330, 135);

        picSanPham.Location = new Point(700, 25); picSanPham.Size = new Size(150, 150);
        picSanPham.BorderStyle = BorderStyle.FixedSingle; picSanPham.SizeMode = PictureBoxSizeMode.Zoom;
        btnChonAnh.Text = "Chọn ảnh"; btnChonAnh.Location = new Point(700, 180); btnChonAnh.Size = new Size(150, 28);
        gb.Controls.AddRange(new Control[] { picSanPham, btnChonAnh });

        btnThem.Text = "Thêm"; btnSua.Text = "Sửa"; btnXoa.Text = "Xóa"; btnLamMoi.Text = "Làm mới"; btnTimKiem.Text = "Tìm kiếm";
        int x = 10;
        foreach (var b in new[] { btnThem, btnSua, btnXoa, btnLamMoi, btnTimKiem })
        {
            b.Location = new Point(x, 278); b.Size = new Size(100, 30);
            x += 110;
            Controls.Add(b);
        }

        dgvProducts.Location = new Point(10, 315);
        dgvProducts.Size = new Size(940, 290);
        dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvProducts.ContextMenuStrip = ctx;

        Controls.AddRange(new Control[] { gb, dgvProducts, toolbar, menu, status });
        MainMenuStrip = menu;

        btnThem.Click += BtnThem_Click;
        btnSua.Click += BtnSua_Click;
        btnXoa.Click += BtnXoa_Click;
        btnLamMoi.Click += BtnLamMoi_Click;
        btnTimKiem.Click += BtnTimKiem_Click;
        btnChonAnh.Click += BtnChonAnh_Click;
        dgvProducts.CellClick += DgvProducts_CellClick;
        dgvProducts.CellMouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvProducts.ClearSelection();
                dgvProducts.Rows[e.RowIndex].Selected = true;
                productSource.Position = e.RowIndex;
            }
        };
    }

    void AddField(GroupBox gb, string text, Control c, int x, int y)
    {
        gb.Controls.Add(new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true });
        c.Location = new Point(x + 80, y);
        c.Width = c is NumericUpDown ? 120 : 200;
        gb.Controls.Add(c);
    }

    void FrmProduct_Load(object sender, EventArgs e)
    {
        cboLoaiSP.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
        cboLoaiSP.SelectedIndex = 0;
        products.Add(new Product { MaSP = "SP01", TenSP = "Chuột không dây", LoaiSP = "Điện tử", DonGia = 150000, SoLuong = 20, NgayNhap = DateTime.Today, ConKinhDoanh = true });
        products.Add(new Product { MaSP = "SP02", TenSP = "Bút bi", LoaiSP = "Văn phòng", DonGia = 5000, SoLuong = 100, NgayNhap = DateTime.Today, ConKinhDoanh = true });
        products.Add(new Product { MaSP = "SP03", TenSP = "Nồi cơm điện", LoaiSP = "Gia dụng", DonGia = 850000, SoLuong = 8, NgayNhap = DateTime.Today.AddDays(-3), ConKinhDoanh = true });
        productSource.DataSource = products;
        dgvProducts.AutoGenerateColumns = true;
        dgvProducts.DataSource = productSource;
        dgvProducts.ReadOnly = true;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.MultiSelect = false;
        dgvProducts.AllowUserToAddRows = false;
        UpdateStatus("Sẵn sàng");
    }

    bool ValidateInput(bool isEdit = false)
    {
        errorProvider1.Clear();
        bool ok = true;
        string ma = txtMaSP.Text.Trim();
        if (ma.Length == 0) { errorProvider1.SetError(txtMaSP, "Mã sản phẩm không được rỗng"); ok = false; }
        else if (!isEdit && products.Any(p => p.MaSP.Equals(ma, StringComparison.OrdinalIgnoreCase))) { errorProvider1.SetError(txtMaSP, "Mã sản phẩm đã tồn tại"); ok = false; }
        if (string.IsNullOrWhiteSpace(txtTenSP.Text)) { errorProvider1.SetError(txtTenSP, "Tên sản phẩm không được rỗng"); ok = false; }
        if (numDonGia.Value <= 0) { errorProvider1.SetError(numDonGia, "Đơn giá phải lớn hơn 0"); ok = false; }
        if (dtpNgayNhap.Value.Date > DateTime.Today) { errorProvider1.SetError(dtpNgayNhap, "Ngày nhập không được lớn hơn hôm nay"); ok = false; }
        return ok;
    }

    Product GetSelectedProduct()
    {
        var p = productSource.Current as Product;
        if (p == null || dgvProducts.CurrentRow == null)
        {
            MessageBox.Show("Vui lòng chọn một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }
        return p;
    }

    void BtnThem_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        products.Add(new Product
        {
            MaSP = txtMaSP.Text.Trim(),
            TenSP = txtTenSP.Text.Trim(),
            LoaiSP = cboLoaiSP.Text,
            DonGia = numDonGia.Value,
            SoLuong = (int)numSoLuong.Value,
            NgayNhap = dtpNgayNhap.Value.Date,
            ConKinhDoanh = chkConKinhDoanh.Checked,
            DuongDanAnh = selectedImagePath
        });
        ClearInput();
        UpdateStatus("Đã thêm sản phẩm mới");
    }

    void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var p = productSource.Current as Product;
        if (p == null) return;
        LoadProductToForm(p);
    }

    void LoadProductToForm(Product p)
    {
        errorProvider1.Clear();
        txtMaSP.Text = p.MaSP;
        txtTenSP.Text = p.TenSP;
        cboLoaiSP.Text = p.LoaiSP;
        numDonGia.Value = Math.Min(p.DonGia, numDonGia.Maximum);
        numSoLuong.Value = p.SoLuong;
        dtpNgayNhap.Value = p.NgayNhap;
        chkConKinhDoanh.Checked = p.ConKinhDoanh;
        selectedImagePath = p.DuongDanAnh;
        SetImage(selectedImagePath);
    }

    void SetImage(string path)
    {
        picSanPham.Image?.Dispose();
        picSanPham.Image = null;
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            using var temp = Image.FromFile(path);
            picSanPham.Image = new Bitmap(temp);
        }
    }

    void BtnSua_Click(object sender, EventArgs e)
    {
        var p = GetSelectedProduct();
        if (p == null) return;
        txtMaSP.Text = p.MaSP;
        if (!ValidateInput(true)) return;
        p.TenSP = txtTenSP.Text.Trim();
        p.LoaiSP = cboLoaiSP.Text;
        p.DonGia = numDonGia.Value;
        p.SoLuong = (int)numSoLuong.Value;
        p.NgayNhap = dtpNgayNhap.Value.Date;
        p.ConKinhDoanh = chkConKinhDoanh.Checked;
        p.DuongDanAnh = selectedImagePath;
        productSource.ResetBindings(false);
        UpdateStatus("Đã cập nhật sản phẩm");
    }

    void BtnXoa_Click(object sender, EventArgs e)
    {
        var p = GetSelectedProduct();
        if (p == null) return;
        if (MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm {p.TenSP}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        products.Remove(p);
        ClearInput();
        UpdateStatus("Đã xóa sản phẩm");
    }

    void BtnTimKiem_Click(object sender, EventArgs e)
    {
        string kw = txtTimKiem.Text.Trim().ToLower();
        var result = products.Where(p => p.MaSP.ToLower().Contains(kw) || p.TenSP.ToLower().Contains(kw)).ToList();
        dgvProducts.DataSource = result;
        lblStatus.Text = $"Tìm thấy {result.Count} sản phẩm | Số sản phẩm: {products.Count}";
    }

    void BtnLamMoi_Click(object sender, EventArgs e)
    {
        dgvProducts.DataSource = productSource;
        ClearInput();
        UpdateStatus("Đã làm mới dữ liệu");
    }

    void BtnChonAnh_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Title = "Chọn ảnh sản phẩm", Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp" };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        try
        {
            SetImage(dlg.FileName);
            selectedImagePath = dlg.FileName;
            UpdateStatus("Đã chọn ảnh sản phẩm");
        }
        catch
        {
            MessageBox.Show("File được chọn không phải ảnh hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    void CmsChiTiet_Click(object sender, EventArgs e)
    {
        var p = GetSelectedProduct();
        if (p == null) return;
        MessageBox.Show($"Mã: {p.MaSP}\nTên: {p.TenSP}\nLoại: {p.LoaiSP}\nTổng tiền: {p.ThanhTien:N0}", "Chi tiết sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    void ClearInput()
    {
        errorProvider1.Clear();
        txtMaSP.Clear();
        txtTenSP.Clear();
        cboLoaiSP.SelectedIndex = 0;
        numDonGia.Value = 1;
        numSoLuong.Value = 0;
        dtpNgayNhap.Value = DateTime.Today;
        chkConKinhDoanh.Checked = true;
        txtTimKiem.Clear();
        SetImage("");
        selectedImagePath = "";
        txtMaSP.Focus();
    }

    void UpdateStatus(string msg) => lblStatus.Text = $"{msg} | Số sản phẩm: {products.Count}";
}
