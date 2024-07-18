using System.Windows;
using System.Windows.Controls;

namespace InventaireCannabis
{
    public partial class UserManagementWindow : Window
    {
        public UserManagementWindow()
        {
            InitializeComponent();
        }

        private void BtnAjouterUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var user = new User
                {
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                context.Users.Add(user);
                context.SaveChanges();

                MessageBox.Show("Utilisateur ajouté avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
