public class FrmMain : Form
{
    MenuStrip menu = new MenuStrip();
    StatusStrip status = new StatusStrip();
    ToolStripStatusLabel lblUser = new ToolStripStatusLabel();
    ToolStripStatusLabel lblTime = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleRight };
    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1000 };
    bool forceClose;
    public bool Logout { get; private set; }

    public FrmMain(User user)
    {
        Text = "ShopManagerMini";
        WindowState = FormWindowState.Maximized;
        IsMdiContainer = true;

        var mSys = new ToolStripMenuItem("Hệ thống");
        mSys.DropDownItems.Add(new ToolStripMenuItem("Đăng xuất", null, (s, e) => DoLogout()));
        mSys.DropDownItems.Add(new ToolStripMenuItem("Thoát", null, (s, e) => Close()));
        var mCat = new ToolStripMenuItem("Danh mục");
        mCat.DropDownItems.Add(new ToolStripMenuItem("Quản lý sản phẩm", null, (s, e) => OpenChild<FrmProduct>()));
        mCat.DropDownItems.Add(new ToolStripMenuItem("Quản lý khách hàng", null, (s, e) => OpenChild<FrmCustomer>()));
        var mHelp = new ToolStripMenuItem("Trợ giúp");
        mHelp.DropDownItems.Add(new ToolStripMenuItem("Giới thiệu", null, (s, e) => OpenChild<FrmAbout>()));
        menu.Items.AddRange(new ToolStripItem[] { mSys, mCat, mHelp });

        lblUser.Text = $"Người dùng: {user.HoTen} | Vai trò: {user.VaiTro}";
        status.Items.AddRange(new ToolStripItem[] { lblUser, lblTime });
        Controls.Add(status);
        Controls.Add(menu);
        MainMenuStrip = menu;


        timer.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        timer.Start();
        FormClosing += FrmMain_FormClosing;
    }

    void OpenChild<T>() where T : Form, new()
    {
        var f = MdiChildren.OfType<T>().FirstOrDefault();
        if (f != null)
        {
            if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
            f.Activate();
            return;
        }
        f = new T { MdiParent = this };
        f.Show();
    }

    void DoLogout()
    {
        if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Logout = true;
        forceClose = true;
        Close();
    }

    void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (forceClose) return;
        if (MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
    }
}
