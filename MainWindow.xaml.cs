using System.Windows;

namespace InventaireCannabis
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAjouterPlantule_Click(object sender, RoutedEventArgs e)
        {
            var window = new PlantuleManagementWindow();
            window.ShowDialog();
        }

        private void BtnRechercherPlantule_Click(object sender, RoutedEventArgs e)
        {
            var window = new SearchPlantuleWindow();
            window.ShowDialog();
        }

        private void BtnGerUser_Click(object sender, RoutedEventArgs e)
        {
            var window = new UserManagementWindow();
            window.ShowDialog();
        }

        private void BtnImporterExcel_Click(object sender, RoutedEventArgs e)
        {
            var importFilePage = new ImportFilePage();
            importFilePage.Show();
        }

        private void BtnAfficherInventaire_Click(object sender, RoutedEventArgs e)
        {
            var window = new InventoryWindow();
            window.ShowDialog();
        }

        private void BtnAfficherRetirees_Click(object sender, RoutedEventArgs e)
        {
            var window = new RetiredPlantulesWindow();
            window.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
