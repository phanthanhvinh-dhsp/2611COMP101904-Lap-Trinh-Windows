using System.Data;

public class FrmNhapDiemTheoLop : Form
{
    KetQuaDAL dal = new KetQuaDAL();
    BindingSource ketQuaSource = new BindingSource();
    ComboBox cboLop = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    ComboBox cboMonHoc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    Button btnTaiDanhSach = new Button { Text = "Tải danh sách" };
    Button btnLuuTatCa = new Button { Text = "Lưu tất cả" };
    DataGridView dgvKetQua = new DataGridView();
    StatusStrip status = new StatusStrip();
    ToolStripStatusLabel lblStatus = new ToolStripStatusLabel("Sẵn sàng");

    public FrmNhapDiemTheoLop()
    {
        Text = "Nhập điểm theo lớp";
        ClientSize = new Size(760, 500);
        StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(new Label { Text = "Lớp:", Location = new Point(15, 20), AutoSize = true });
        cboLop.SetBounds(60, 16, 200, 25);
        Controls.Add(new Label { Text = "Môn học:", Location = new Point(280, 20), AutoSize = true });
        cboMonHoc.SetBounds(345, 16, 200, 25);
        btnTaiDanhSach.SetBounds(560, 14, 180, 30);
        dgvKetQua.SetBounds(15, 60, 730, 370);
        dgvKetQua.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvKetQua.AllowUserToAddRows = false;
        btnLuuTatCa.SetBounds(15, 440, 140, 32);
        btnLuuTatCa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        status.Items.Add(lblStatus);
        Controls.AddRange(new Control[] { cboLop, cboMonHoc, btnTaiDanhSach, dgvKetQua, btnLuuTatCa, status });

        Load += FrmNhapDiemTheoLop_Load;
        btnTaiDanhSach.Click += BtnTaiDanhSach_Click;
        btnLuuTatCa.Click += BtnLuuTatCa_Click;
    }

    void FrmNhapDiemTheoLop_Load(object sender, EventArgs e)
    {
        try
        {
            LoadComboBox();
            dgvKetQua.AutoGenerateColumns = true;
            dgvKetQua.DataSource = ketQuaSource;
        }
        catch (Exception ex) { MessageBox.Show("Không thể kết nối database: " + ex.Message); }
    }

    void LoadComboBox()
    {
        cboLop.DisplayMember = "TenLop";
        cboLop.ValueMember = "MaLop";
        cboLop.DataSource = dal.GetLop();
        cboMonHoc.DisplayMember = "TenMon";
        cboMonHoc.ValueMember = "MaMon";
        cboMonHoc.DataSource = dal.GetMonHoc();
    }

    void BtnTaiDanhSach_Click(object sender, EventArgs e)
    {
        string maLop = cboLop.SelectedValue?.ToString() ?? "";
        string maMon = cboMonHoc.SelectedValue?.ToString() ?? "";
        if (maLop.Length == 0 || maMon.Length == 0)
        {
            MessageBox.Show("Vui lòng chọn lớp và môn học.");
            return;
        }
        try
        {
            ketQuaSource.DataSource = dal.GetKetQuaByLopMon(maLop, maMon);
            dgvKetQua.Columns["MaSV"].ReadOnly = true;
            dgvKetQua.Columns["HoTen"].ReadOnly = true;
            lblStatus.Text = $"Đã tải {ketQuaSource.Count} sinh viên";
        }
        catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
    }

    void BtnLuuTatCa_Click(object sender, EventArgs e)
    {
        if (ketQuaSource.DataSource is not DataTable table)
        {
            MessageBox.Show("Vui lòng tải danh sách trước.");
            return;
        }
        try
        {
            dgvKetQua.EndEdit();
            ketQuaSource.EndEdit();
            ValidateData(table);
            int count = dal.SaveAll(cboMonHoc.SelectedValue.ToString(), table);
            lblStatus.Text = $"Đã lưu {count} dòng";
            MessageBox.Show($"Đã lưu thành công {count} dòng.");
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Lưu thất bại, đã rollback";
            MessageBox.Show("Không thể lưu dữ liệu: " + ex.Message);
        }
    }

    void ValidateData(DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            string maSV = row["MaSV"].ToString();
            string text = row["Diem"].ToString();
            if (string.IsNullOrWhiteSpace(text)) throw new Exception($"Điểm của sinh viên {maSV} không được rỗng.");
            if (!double.TryParse(text, out double diem)) throw new Exception($"Điểm của sinh viên {maSV} không hợp lệ.");
            if (diem < 0 || diem > 10) throw new Exception($"Điểm của sinh viên {maSV} phải từ 0 đến 10.");
        }
    }
}
