public class FrmAbout : Form
{
    public FrmAbout()
    {
        Text = "Giới thiệu";
        ClientSize = new Size(360, 180);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Controls.Add(new Label
        {
            Text = "ShopManagerMini\nỨng dụng quản lý cửa hàng mini\n\nSinh viên: [Họ tên - MSSV]\nLớp: [Tên lớp]\nHọc phần: COMP1019 - Lập trình trên Windows",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        });
    }
}
