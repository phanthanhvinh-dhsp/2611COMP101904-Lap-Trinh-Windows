using System.Drawing.Drawing2D;
using System.Drawing.Printing;

public class FrmStudentCard : Form
{
    TextBox txtStudentId = new TextBox();
    TextBox txtFullName = new TextBox();
    ComboBox cboClass = new ComboBox();
    ComboBox cboFaculty = new ComboBox();
    TextBox txtCourse = new TextBox();
    DateTimePicker dtpBirthDate = new DateTimePicker();
    ComboBox cboGender = new ComboBox();
    PictureBox picAvatar = new PictureBox();
    Panel pnlCardPreview = new BufferedPanel();
    Button btnChoosePhoto = new Button { Text = "Chọn ảnh" };
    Button btnPreviewPrint = new Button { Text = "Xem trước in" };
    Button btnPrint = new Button { Text = "In" };
    Button btnClear = new Button { Text = "Làm mới" };
    Button btnExit = new Button { Text = "Thoát" };
    PrintDocument printDocument1 = new PrintDocument();
    PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
    PrintDialog printDialog1 = new PrintDialog();

    public FrmStudentCard()
    {
        Text = "Thẻ sinh viên";
        ClientSize = new Size(1000, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BuildUI();
        InitData();
        RegisterEvents();
    }

    void BuildUI()
    {
        var gb = new GroupBox { Text = "Thông tin sinh viên", Location = new Point(10, 10), Size = new Size(400, 500) };
        Field(gb, "Mã SV:", txtStudentId, 30);
        Field(gb, "Họ tên:", txtFullName, 65);
        cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
        Field(gb, "Lớp:", cboClass, 100);
        cboFaculty.DropDownStyle = ComboBoxStyle.DropDownList;
        Field(gb, "Khoa:", cboFaculty, 135);
        Field(gb, "Khóa:", txtCourse, 170);
        dtpBirthDate.Format = DateTimePickerFormat.Short;
        Field(gb, "Ngày sinh:", dtpBirthDate, 205);
        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        Field(gb, "Giới tính:", cboGender, 240);
        picAvatar.SetBounds(120, 280, 110, 140);
        picAvatar.BorderStyle = BorderStyle.FixedSingle;
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        btnChoosePhoto.SetBounds(245, 280, 130, 30);
        gb.Controls.AddRange(new Control[] { picAvatar, btnChoosePhoto });
        btnPreviewPrint.SetBounds(15, 440, 110, 32);
        btnPrint.SetBounds(135, 440, 80, 32);
        btnClear.SetBounds(225, 440, 80, 32);
        btnExit.SetBounds(315, 440, 70, 32);
        gb.Controls.AddRange(new Control[] { btnPreviewPrint, btnPrint, btnClear, btnExit });

        pnlCardPreview.Location = new Point(420, 10);
        pnlCardPreview.Size = new Size(570, 400);
        pnlCardPreview.BackColor = Color.White;
        pnlCardPreview.BorderStyle = BorderStyle.FixedSingle;
        Controls.AddRange(new Control[] { gb, pnlCardPreview });
    }

    void Field(GroupBox gb, string text, Control c, int y)
    {
        gb.Controls.Add(new Label { Text = text, Location = new Point(15, y + 3), AutoSize = true });
        c.SetBounds(105, y, 270, 25);
        gb.Controls.Add(c);
    }

    void InitData()
    {
        cboClass.Items.AddRange(new object[] { "K49.CNTT.A", "K50.CNTT.A", "K51.CNTT.A" });
        cboFaculty.Items.AddRange(new object[] { "Công nghệ thông tin", "Toán - Tin học" });
        cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
        cboClass.SelectedIndex = 0;
        cboFaculty.SelectedIndex = 0;
        cboGender.SelectedIndex = 0;
        printPreviewDialog1.Document = printDocument1;
        printPreviewDialog1.Width = 900;
        printPreviewDialog1.Height = 700;
        printDialog1.Document = printDocument1;
    }

    void RegisterEvents()
    {
        btnChoosePhoto.Click += BtnChoosePhoto_Click;
        btnPreviewPrint.Click += BtnPreviewPrint_Click;
        btnPrint.Click += BtnPrint_Click;
        btnClear.Click += BtnClear_Click;
        btnExit.Click += (s, e) => Close();
        pnlCardPreview.Paint += PnlCardPreview_Paint;
        printDocument1.PrintPage += PrintDocument1_PrintPage;
        txtStudentId.TextChanged += InputChanged;
        txtFullName.TextChanged += InputChanged;
        txtCourse.TextChanged += InputChanged;
        cboClass.SelectedIndexChanged += InputChanged;
        cboFaculty.SelectedIndexChanged += InputChanged;
        cboGender.SelectedIndexChanged += InputChanged;
        dtpBirthDate.ValueChanged += InputChanged;
    }

    void InputChanged(object sender, EventArgs e) => pnlCardPreview.Invalidate();

    void BtnChoosePhoto_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Title = "Chọn ảnh sinh viên", Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp" };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        try
        {
            using var temp = Image.FromFile(dlg.FileName);
            var copy = new Bitmap(temp);
            picAvatar.Image?.Dispose();
            picAvatar.Image = copy;
            pnlCardPreview.Invalidate();
        }
        catch
        {
            MessageBox.Show("File được chọn không phải ảnh hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    StudentCardInfo GetInfo() => new StudentCardInfo
    {
        StudentId = txtStudentId.Text.Trim(),
        FullName = txtFullName.Text.Trim(),
        ClassName = cboClass.Text,
        Faculty = cboFaculty.Text,
        Course = txtCourse.Text.Trim(),
        BirthDate = dtpBirthDate.Value,
        Gender = cboGender.Text,
        Photo = picAvatar.Image
    };

    bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtStudentId.Text)) { MessageBox.Show("Vui lòng nhập mã sinh viên."); txtStudentId.Focus(); return false; }
        if (string.IsNullOrWhiteSpace(txtFullName.Text)) { MessageBox.Show("Vui lòng nhập họ tên."); txtFullName.Focus(); return false; }
        if (picAvatar.Image == null) { MessageBox.Show("Vui lòng chọn ảnh sinh viên."); return false; }
        return true;
    }

    void PnlCardPreview_Paint(object sender, PaintEventArgs e)
    {
        var card = new Rectangle(20, 20, 520, 320);
        DrawCard(e.Graphics, card, GetInfo());
    }

    void DrawCard(Graphics g, Rectangle card, StudentCardInfo s)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var f1 = new Font("Arial", 10, FontStyle.Bold);
        using var f2 = new Font("Arial", 16, FontStyle.Bold);
        using var f3 = new Font("Arial", 10, FontStyle.Regular);
        using var pen = new Pen(Color.Navy, 2);
        using var navy = new SolidBrush(Color.Navy);
        using var bg = new SolidBrush(Color.FromArgb(238, 247, 247));
        g.FillRectangle(bg, card);
        g.DrawRectangle(pen, card);
        int x = card.Left, y = card.Top;
        g.DrawString("TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH", f1, navy, x + 20, y + 15);
        g.DrawString("THẺ SINH VIÊN", f2, navy, x + 20, y + 45);
        var photo = new Rectangle(x + 25, y + 90, 115, 150);
        g.FillRectangle(Brushes.White, photo);
        g.DrawRectangle(Pens.Gray, photo);
        if (s.Photo != null) DrawZoom(g, s.Photo, photo);
        else g.DrawString("Ảnh", f3, Brushes.Gray, photo.Left + 35, photo.Top + 65);
        int ix = x + 165, iy = y + 95;
        Line(g, "MSSV:", s.StudentId, f1, f3, ix, iy);
        Line(g, "Họ tên:", s.FullName, f1, f3, ix, iy + 30);
        Line(g, "Lớp:", s.ClassName, f1, f3, ix, iy + 60);
        Line(g, "Khoa:", s.Faculty, f1, f3, ix, iy + 90);
        Line(g, "Khóa:", s.Course, f1, f3, ix, iy + 120);
        Line(g, "Ngày sinh:", s.BirthDate.ToString("dd/MM/yyyy"), f1, f3, ix, iy + 150);
        Line(g, "Giới tính:", s.Gender, f1, f3, ix, iy + 180);
    }

    void Line(Graphics g, string label, string value, Font lf, Font nf, int x, int y)
    {
        g.DrawString(label, lf, Brushes.Black, x, y);
        g.DrawString(value, nf, Brushes.Black, x + 90, y);
    }

    void DrawZoom(Graphics g, Image img, Rectangle box)
    {
        float ratio = Math.Min((float)box.Width / img.Width, (float)box.Height / img.Height);
        int w = (int)(img.Width * ratio), h = (int)(img.Height * ratio);
        g.DrawImage(img, new Rectangle(box.Left + (box.Width - w) / 2, box.Top + (box.Height - h) / 2, w, h));
    }

    void BtnPreviewPrint_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        printPreviewDialog1.ShowDialog();
    }

    void BtnPrint_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        if (printDialog1.ShowDialog() == DialogResult.OK) printDocument1.Print();
    }

    void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
    {
        var card = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, 520, 320);
        DrawCard(e.Graphics, card, GetInfo());
        e.HasMorePages = false;
    }

    void BtnClear_Click(object sender, EventArgs e)
    {
        txtStudentId.Clear();
        txtFullName.Clear();
        txtCourse.Clear();
        cboClass.SelectedIndex = 0;
        cboFaculty.SelectedIndex = 0;
        cboGender.SelectedIndex = 0;
        dtpBirthDate.Value = DateTime.Today;
        picAvatar.Image?.Dispose();
        picAvatar.Image = null;
        pnlCardPreview.Invalidate();
        txtStudentId.Focus();
    }
}

class BufferedPanel : Panel
{
    public BufferedPanel() { DoubleBuffered = true; }
}
