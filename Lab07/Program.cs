ApplicationConfiguration.Initialize();
while (true)
{
    var login = new FrmLogin();
    if (login.ShowDialog() != DialogResult.OK) break;
    var main = new FrmMain(login.CurrentUser);
    Application.Run(main);
    if (!main.Logout) break;
}
