using System.Text;

public class FrmBaoCaoSinhVien : Form
{
    List<SinhVien> danhSachSinhVien = new List<SinhVien>();
    ComboBox cboLop = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    ComboBox cboKetQua = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    TextBox txtTuKhoa = new TextBox();
    Button btnXemBaoCao = new Button { Text = "Xem báo cáo" };
    Button btnThongKe = new Button { Text = "Thống kê" };
    Button btnXuatFile = new Button { Text = "Xuất file" };
    DataGridView dgvBaoCao = new DataGridView();
    Label lblThongKe = new Label();
    bool loading;

    public FrmBaoCaoSinhVien()
    {
        Text = "Báo cáo sinh viên";
        ClientSize = new Size(800, 520);
        StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(new Label { Text = "Lớp:", Location = new Point(15, 20), AutoSize = true });
        cboLop.SetBounds(55, 16, 150, 25);
        Controls.Add(new Label { Text = "Kết quả:", Location = new Point(220, 20), AutoSize = true });
        cboKetQua.SetBounds(280, 16, 110, 25);
        Controls.Add(new Label { Text = "Từ khóa:", Location = new Point(405, 20), AutoSize = true });
        txtTuKhoa.SetBounds(470, 16, 150, 25);
        btnXemBaoCao.SetBounds(15, 55, 120, 30);
        btnThongKe.SetBounds(145, 55, 120, 30);
        btnXuatFile.SetBounds(275, 55, 120, 30);
        dgvBaoCao.SetBounds(15, 100, 770, 370);
        dgvBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvBaoCao.ReadOnly = true;
        dgvBaoCao.AllowUserToAddRows = false;
        lblThongKe.SetBounds(15, 480, 770, 25);
        lblThongKe.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Controls.AddRange(new Control[] { cboLop, cboKetQua, txtTuKhoa, btnXemBaoCao, btnThongKe, btnXuatFile, dgvBaoCao, lblThongKe });

        Load += FrmBaoCaoSinhVien_Load;
        btnXemBaoCao.Click += (s, e) => XemBaoCao();
        btnThongKe.Click += BtnThongKe_Click;
        btnXuatFile.Click += BtnXuatFile_Click;
        cboLop.SelectedIndexChanged += (s, e) => { if (!loading) XemBaoCao(); };
        cboKetQua.SelectedIndexChanged += (s, e) => { if (!loading) XemBaoCao(); };
        txtTuKhoa.TextChanged += (s, e) => { if (!loading) XemBaoCao(); };
    }

    void FrmBaoCaoSinhVien_Load(object sender, EventArgs e)
    {
        TaoDuLieuMau();
        LoadComboBox();
        XemBaoCao();
    }

    void TaoDuLieuMau()
    {
        danhSachSinhVien = new List<SinhVien>
        {
            new SinhVien { MaSV = "SV001", HoTen = "Nguyễn Văn An", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 8.2 },
            new SinhVien { MaSV = "SV002", HoTen = "Trần Thị Bình", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 6.5 },
            new SinhVien { MaSV = "SV003", HoTen = "Lê Minh Cường", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 4.8 },
            new SinhVien { MaSV = "SV004", HoTen = "Phạm Thanh Dung", MaLop = "CTK47A", TenLop = "CNTT K47A", Diem = 9.1 },
            new SinhVien { MaSV = "SV005", HoTen = "Hoàng Quốc Huy", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 7.4 },
            new SinhVien { MaSV = "SV006", HoTen = "Võ Gia Khang", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 3.9 },
            new SinhVien { MaSV = "SV007", HoTen = "Đặng Minh Khôi", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 5.5 },
            new SinhVien { MaSV = "SV008", HoTen = "Bùi Thảo Linh", MaLop = "CTK47B", TenLop = "CNTT K47B", Diem = 8.0 },
            new SinhVien { MaSV = "SV009", HoTen = "Ngô Tuấn Minh", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 6.0 },
            new SinhVien { MaSV = "SV010", HoTen = "Đỗ Bảo Ngọc", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 9.3 },
            new SinhVien { MaSV = "SV011", HoTen = "Huỳnh Gia Phúc", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 4.2 },
            new SinhVien { MaSV = "SV012", HoTen = "Mai Anh Thư", MaLop = "CTK48A", TenLop = "CNTT K48A", Diem = 7.8 }
        };
    }

    void LoadComboBox()
    {
        loading = true;
        var dsLop = danhSachSinhVien.Select(sv => new { sv.MaLop, sv.TenLop }).Distinct().OrderBy(l => l.MaLop).ToList();
        cboLop.DisplayMember = "TenLop";
        cboLop.ValueMember = "MaLop";
        cboLop.DataSource = dsLop;
        cboKetQua.Items.Clear();
        cboKetQua.Items.AddRange(new object[] { "Tất cả", "Đạt", "Không đạt" });
        cboKetQua.SelectedIndex = 0;
        loading = false;
    }

    void XemBaoCao()
    {
        if (cboLop.SelectedValue == null) return;
        string maLop = cboLop.SelectedValue.ToString();
        string ketQua = cboKetQua.SelectedItem?.ToString() ?? "Tất cả";
        string tuKhoa = txtTuKhoa.Text.Trim().ToLower();

        var query = danhSachSinhVien.Where(sv => sv.MaLop == maLop);
        if (ketQua == "Đạt") query = query.Where(sv => sv.Diem >= 5);
        else if (ketQua == "Không đạt") query = query.Where(sv => sv.Diem < 5);
        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(sv => sv.MaSV.ToLower().Contains(tuKhoa) || sv.HoTen.ToLower().Contains(tuKhoa));

        var baoCao = query
            .OrderByDescending(sv => sv.Diem)
            .ThenBy(sv => sv.HoTen)
            .Select(sv => new { sv.MaSV, sv.HoTen, sv.TenLop, sv.Diem, sv.KetQua })
            .ToList();

        dgvBaoCao.DataSource = baoCao;
        CapNhatThongKe(baoCao.Count);
        if (baoCao.Count == 0) MessageBox.Show("Không có dữ liệu phù hợp.");
    }

    void CapNhatThongKe(int soDong)
    {
        string maLop = cboLop.SelectedValue?.ToString() ?? "";
        var dsLop = danhSachSinhVien.Where(sv => sv.MaLop == maLop).ToList();
        int tong = dsLop.Count;
        int soDat = dsLop.Count(sv => sv.Diem >= 5);
        int soKhongDat = dsLop.Count(sv => sv.Diem < 5);
        double dtb = tong > 0 ? dsLop.Average(sv => sv.Diem) : 0;
        lblThongKe.Text = $"Đang hiển thị: {soDong} | Tổng lớp: {tong} | Đạt: {soDat} | Không đạt: {soKhongDat} | ĐTB: {dtb:0.00}";
    }

    void BtnThongKe_Click(object sender, EventArgs e)
    {
        var thongKe = danhSachSinhVien
            .GroupBy(sv => new { sv.MaLop, sv.TenLop })
            .Select(g => new
            {
                g.Key.MaLop,
                g.Key.TenLop,
                SoLuong = g.Count(),
                SoDat = g.Count(sv => sv.Diem >= 5),
                SoKhongDat = g.Count(sv => sv.Diem < 5),
                DiemTrungBinh = Math.Round(g.Average(sv => sv.Diem), 2)
            })
            .OrderBy(x => x.MaLop)
            .ToList();
        dgvBaoCao.DataSource = thongKe;
        lblThongKe.Text = "Thống kê theo lớp";
    }

    void BtnXuatFile_Click(object sender, EventArgs e)
    {
        if (dgvBaoCao.Rows.Count == 0)
        {
            MessageBox.Show("Không có dữ liệu để xuất.");
            return;
        }
        using var sfd = new SaveFileDialog { Filter = "CSV file (*.csv)|*.csv|Text file (*.txt)|*.txt", FileName = "BaoCaoSinhVien.csv" };
        if (sfd.ShowDialog() != DialogResult.OK) return;
        try
        {
            var sb = new StringBuilder();
            int n = dgvBaoCao.Columns.Count;
            sb.AppendLine(string.Join(",", dgvBaoCao.Columns.Cast<DataGridViewColumn>().Select(c => Csv(c.HeaderText))));
            foreach (DataGridViewRow row in dgvBaoCao.Rows)
            {
                if (row.IsNewRow) continue;
                sb.AppendLine(string.Join(",", Enumerable.Range(0, n).Select(i => Csv(row.Cells[i].Value?.ToString() ?? ""))));
            }
            File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
            MessageBox.Show("Xuất file thành công.");
        }
        catch (Exception ex) { MessageBox.Show("Lỗi xuất file: " + ex.Message); }
    }

    static string Csv(string v) => v.Contains(',') || v.Contains('"') ? "\"" + v.Replace("\"", "\"\"") + "\"" : v;
}
