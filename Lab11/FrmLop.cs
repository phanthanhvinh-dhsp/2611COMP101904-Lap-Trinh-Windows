using System.Data;
using Microsoft.Data.SqlClient;

public class FrmLop : Form
{
    const string connectionString = @"Data Source=.;Initial Catalog=EduManagerDB;Integrated Security=True;TrustServerCertificate=True";

    Label lblMaLop = new Label { Text = "Mã lớp:", AutoSize = true };
    Label lblTenLop = new Label { Text = "Tên lớp:", AutoSize = true };
    TextBox txtMaLop = new TextBox();
    TextBox txtTenLop = new TextBox();
    TextBox txtTimKiem = new TextBox();
    DataGridView dgvLop = new DataGridView();
    Button btnThem = new Button { Text = "Thêm" };
    Button btnSua = new Button { Text = "Sửa" };
    Button btnXoa = new Button { Text = "Xóa" };
    Button btnLamMoi = new Button { Text = "Làm mới" };
    Button btnTimKiem = new Button { Text = "Tìm kiếm" };

    public FrmLop()
    {
        Text = "Quản lý lớp học";
        ClientSize = new Size(720, 480);
        StartPosition = FormStartPosition.CenterScreen;
        lblMaLop.Location = new Point(20, 23);
        txtMaLop.SetBounds(90, 20, 200, 25);
        lblTenLop.Location = new Point(20, 58);
        txtTenLop.SetBounds(90, 55, 200, 25);
        txtTimKiem.SetBounds(400, 20, 200, 25);
        btnTimKiem.SetBounds(610, 18, 90, 28);
        Controls.AddRange(new Control[] { lblMaLop, txtMaLop, lblTenLop, txtTenLop, txtTimKiem, btnTimKiem });
        int x = 20;
        foreach (var b in new[] { btnThem, btnSua, btnXoa, btnLamMoi })
        {
            b.SetBounds(x, 100, 100, 30);
            x += 110;
            Controls.Add(b);
        }
        dgvLop.SetBounds(20, 145, 680, 315);
        dgvLop.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvLop.ReadOnly = true;
        dgvLop.AllowUserToAddRows = false;
        dgvLop.MultiSelect = false;
        dgvLop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        Controls.Add(dgvLop);

        Load += (s, e) => LoadData();
        btnThem.Click += BtnThem_Click;
        btnSua.Click += BtnSua_Click;
        btnXoa.Click += BtnXoa_Click;
        btnTimKiem.Click += BtnTimKiem_Click;
        btnLamMoi.Click += (s, e) => { ClearInput(); LoadData(); };
        dgvLop.CellClick += DgvLop_CellClick;
    }

    const string SelectSql = @"SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
{0}
GROUP BY l.MaLop, l.TenLop
ORDER BY l.MaLop";

    void Fill(string where, string kw)
    {
        using var conn = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(string.Format(SelectSql, where), conn);
        if (kw != null) cmd.Parameters.AddWithValue("@kw", kw);
        using var da = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        da.Fill(dt);
        dgvLop.DataSource = dt;
    }

    void LoadData()
    {
        try { Fill("", null); }
        catch (Exception ex) { MessageBox.Show("Lỗi load dữ liệu: " + ex.Message); }
    }

    bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtMaLop.Text)) { MessageBox.Show("Vui lòng nhập mã lớp."); txtMaLop.Focus(); return false; }
        if (string.IsNullOrWhiteSpace(txtTenLop.Text)) { MessageBox.Show("Vui lòng nhập tên lớp."); txtTenLop.Focus(); return false; }
        return true;
    }

    int Scalar(string sql, string ma)
    {
        using var conn = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", ma);
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    int Exec(string sql, string ma, string ten = null)
    {
        using var conn = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", ma);
        if (ten != null) cmd.Parameters.AddWithValue("@ten", ten);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    void BtnThem_Click(object sender, EventArgs e)
    {
        if (txtMaLop.ReadOnly) { MessageBox.Show("Bấm Làm mới trước khi thêm lớp mới."); return; }
        if (!ValidateInput()) return;
        string ma = txtMaLop.Text.Trim(), ten = txtTenLop.Text.Trim();
        try
        {
            if (Scalar("SELECT COUNT(*) FROM LOP WHERE MaLop = @ma", ma) > 0) { MessageBox.Show("Mã lớp đã tồn tại."); return; }
            if (Exec("INSERT INTO LOP(MaLop, TenLop) VALUES(@ma, @ten)", ma, ten) > 0)
            {
                MessageBox.Show("Thêm lớp thành công.");
                ClearInput();
                LoadData();
            }
        }
        catch (Exception ex) { MessageBox.Show("Lỗi thêm lớp: " + ex.Message); }
    }

    void BtnSua_Click(object sender, EventArgs e)
    {
        if (!txtMaLop.ReadOnly) { MessageBox.Show("Vui lòng chọn lớp cần sửa."); return; }
        if (!ValidateInput()) return;
        try
        {
            if (Exec("UPDATE LOP SET TenLop = @ten WHERE MaLop = @ma", txtMaLop.Text.Trim(), txtTenLop.Text.Trim()) > 0)
            {
                MessageBox.Show("Cập nhật lớp thành công.");
                ClearInput();
                LoadData();
            }
            else MessageBox.Show("Không tìm thấy lớp cần sửa.");
        }
        catch (Exception ex) { MessageBox.Show("Lỗi cập nhật: " + ex.Message); }
    }

    void BtnXoa_Click(object sender, EventArgs e)
    {
        string ma = txtMaLop.Text.Trim();
        if (ma.Length == 0) { MessageBox.Show("Vui lòng chọn lớp cần xóa."); return; }
        try
        {
            if (Scalar("SELECT COUNT(*) FROM SINHVIEN WHERE MaLop = @ma", ma) > 0) { MessageBox.Show("Không thể xóa lớp đang có sinh viên."); return; }
            if (MessageBox.Show("Bạn có chắc muốn xóa lớp này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (Exec("DELETE FROM LOP WHERE MaLop = @ma", ma) > 0)
            {
                MessageBox.Show("Xóa lớp thành công.");
                ClearInput();
                LoadData();
            }
        }
        catch (Exception ex) { MessageBox.Show("Lỗi xóa lớp: " + ex.Message); }
    }

    void BtnTimKiem_Click(object sender, EventArgs e)
    {
        try { Fill("WHERE l.MaLop LIKE @kw OR l.TenLop LIKE @kw", "%" + txtTimKiem.Text.Trim() + "%"); }
        catch (Exception ex) { MessageBox.Show("Lỗi tìm kiếm: " + ex.Message); }
    }

    void DgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dgvLop.Rows[e.RowIndex];
        txtMaLop.Text = row.Cells["MaLop"].Value.ToString();
        txtTenLop.Text = row.Cells["TenLop"].Value.ToString();
        txtMaLop.ReadOnly = true;
    }

    void ClearInput()
    {
        txtMaLop.Clear();
        txtTenLop.Clear();
        txtTimKiem.Clear();
        txtMaLop.ReadOnly = false;
        txtMaLop.Focus();
    }
}
