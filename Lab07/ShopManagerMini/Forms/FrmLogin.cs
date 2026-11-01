public class FrmLogin : Form
{
    TextBox txtUsername = new TextBox();
    TextBox txtPassword = new TextBox();
    Button btnDangNhap = new Button();
    Button btnThoat = new Button();
    public User CurrentUser { get; private set; }

    public FrmLogin()
    {
        Text = "Đăng nhập";
        ClientSize = new Size(340, 190);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        txtPassword.UseSystemPasswordChar = true;
        Controls.Add(new Label { Text = "Username:", Location = new Point(20, 28), AutoSize = true });
        Controls.Add(new Label { Text = "Password:", Location = new Point(20, 68), AutoSize = true });
        txtUsername.SetBounds(100, 25, 210, 25);
        txtPassword.SetBounds(100, 65, 210, 25);
        btnDangNhap.Text = "Đăng nhập"; btnDangNhap.SetBounds(60, 120, 110, 32);
        btnThoat.Text = "Thoát"; btnThoat.SetBounds(190, 120, 110, 32);
        Controls.AddRange(new Control[] { txtUsername, txtPassword, btnDangNhap, btnThoat });
        AcceptButton = btnDangNhap;
        CancelButton = btnThoat;
        btnDangNhap.Click += BtnDangNhap_Click;
        btnThoat.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    void BtnDangNhap_Click(object sender, EventArgs e)
    {
        string u = txtUsername.Text.Trim();
        string p = txtPassword.Text;
        if (u.Length == 0 || p.Length == 0)
        {
            MessageBox.Show("Vui lòng nhập username và password.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var user = Data.Users.FirstOrDefault(x => x.Username == u && x.Password == p);
        if (user == null)
        {
            MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPassword.Clear();
            txtPassword.Focus();
            return;
        }
        CurrentUser = user;
        DialogResult = DialogResult.OK;
        Close();
    }
}
