using System.Drawing.Drawing2D;

public class FrmPaint : Form
{
    DrawSurface canvas = new DrawSurface();
    ComboBox cboMode = new ComboBox();
    NumericUpDown nudWidth = new NumericUpDown();
    Button btnColor = new Button();
    Button btnUndo = new Button();
    Button btnClear = new Button();
    Label lblStatus = new Label();
    List<DrawItem> items = new List<DrawItem>();
    DrawItem currentItem;
    bool isDrawing;
    Color currentColor = Color.Black;

    public FrmPaint()
    {
        Text = "COMP1019 - Simple Paint";
        Size = new Size(1000, 650);
        StartPosition = FormStartPosition.CenterScreen;
        BuildUI();
        canvas.Paint += Canvas_Paint;
        canvas.MouseDown += Canvas_MouseDown;
        canvas.MouseMove += Canvas_MouseMove;
        canvas.MouseUp += Canvas_MouseUp;
        btnColor.Click += BtnColor_Click;
        btnUndo.Click += (s, e) => Undo();
        btnClear.Click += BtnClear_Click;
        KeyPreview = true;
        KeyDown += (s, e) => { if (e.Control && e.KeyCode == Keys.Z) Undo(); };
    }

    void BuildUI()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.FromArgb(238, 247, 247) };
        cboMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMode.Items.AddRange(new object[] { "Pen", "Line", "Rectangle", "Ellipse" });
        cboMode.SelectedIndex = 0;
        cboMode.SetBounds(70, 12, 110, 25);
        nudWidth.Minimum = 1;
        nudWidth.Maximum = 20;
        nudWidth.Value = 3;
        nudWidth.SetBounds(250, 12, 60, 25);
        btnColor.Text = "Color"; btnColor.SetBounds(330, 10, 80, 28);
        btnUndo.Text = "Undo"; btnUndo.SetBounds(430, 10, 80, 28);
        btnClear.Text = "Clear"; btnClear.SetBounds(530, 10, 80, 28);
        top.Controls.Add(new Label { Text = "Mode", Left = 15, Top = 16, AutoSize = true });
        top.Controls.Add(cboMode);
        top.Controls.Add(new Label { Text = "Width", Left = 200, Top = 16, AutoSize = true });
        top.Controls.AddRange(new Control[] { nudWidth, btnColor, btnUndo, btnClear });
        lblStatus.Dock = DockStyle.Bottom;
        lblStatus.Height = 26;
        lblStatus.Text = "Ready";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        Controls.Add(canvas);
        Controls.Add(lblStatus);
        Controls.Add(top);
    }

    DrawMode CurrentMode => (DrawMode)Enum.Parse(typeof(DrawMode), cboMode.SelectedItem.ToString());

    void BtnColor_Click(object sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = currentColor };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        currentColor = dlg.Color;
        btnColor.BackColor = currentColor;
    }

    void Undo()
    {
        if (items.Count == 0) return;
        items.RemoveAt(items.Count - 1);
        canvas.Invalidate();
    }

    void BtnClear_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Xóa toàn bộ vùng vẽ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        items.Clear();
        canvas.Invalidate();
    }

    void Canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        isDrawing = true;
        currentItem = new DrawItem { Mode = CurrentMode, Start = e.Location, End = e.Location, Color = currentColor, Width = (float)nudWidth.Value };
        if (currentItem.Mode == DrawMode.Pen) currentItem.Points.Add(e.Location);
    }

    void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        lblStatus.Text = $"X = {e.X}, Y = {e.Y}, Items = {items.Count}";
        if (!isDrawing || currentItem == null) return;
        currentItem.End = e.Location;
        if (currentItem.Mode == DrawMode.Pen) currentItem.Points.Add(e.Location);
        canvas.Invalidate();
    }

    void Canvas_MouseUp(object sender, MouseEventArgs e)
    {
        if (!isDrawing || currentItem == null) return;
        currentItem.End = e.Location;
        items.Add(currentItem);
        currentItem = null;
        isDrawing = false;
        canvas.Invalidate();
    }

    void Canvas_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        foreach (var item in items) DrawOne(e.Graphics, item);
        if (currentItem != null) DrawOne(e.Graphics, currentItem);
    }

    void DrawOne(Graphics g, DrawItem item)
    {
        using var pen = new Pen(item.Color, item.Width) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        var r = Rect(item.Start, item.End);
        switch (item.Mode)
        {
            case DrawMode.Pen:
                for (int i = 1; i < item.Points.Count; i++) g.DrawLine(pen, item.Points[i - 1], item.Points[i]);
                break;
            case DrawMode.Line: g.DrawLine(pen, item.Start, item.End); break;
            case DrawMode.Rectangle: g.DrawRectangle(pen, r); break;
            case DrawMode.Ellipse: g.DrawEllipse(pen, r); break;
        }
    }

    Rectangle Rect(Point a, Point b) => new Rectangle(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
}
