using System.Linq;
using System.Windows;

namespace InventaireCannabis
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == txtEmail.Text && u.Password == txtPassword.Password);
                if (user != null)
                {
                    if (user.Status == "Admin")
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                    }
                    else
                    {
                        var employeeWindow = new EmployeeWindow();
                        employeeWindow.Show();
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                }
            }
        }
    }
}
