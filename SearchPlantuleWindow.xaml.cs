using System.Linq;
using System.Windows;

namespace InventaireCannabis
{
    public partial class SearchPlantuleWindow : Window
    {
        private Plantule _plantule;

        public SearchPlantuleWindow()
        {
            InitializeComponent();
        }

        private void BtnRechercherPlantule_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var plantuleId = txtSearchId.Text;
                _plantule = context.Plantules.FirstOrDefault(p => p.PlantuleId == plantuleId);

                if (_plantule != null)
                {
                    txtPlantuleInfo.Text = $"ID: {_plantule.PlantuleId}\n" +
                                           $"Date d'arrivée: {_plantule.DateArrivee}\n" +
                                           $"État de santé: {ConvertHexToColorName(_plantule.EtatSante)}\n" +
                                           $"Provenance: {_plantule.Provenance}\n" +
                                           $"Description: {_plantule.Description}\n" +
                                           $"Entreposage: {_plantule.Entreposage}\n" +
                                           $"Note: {_plantule.Note}\n" +
                                           $"Stade: {_plantule.Stade}\n" +
                                           $"Responsable de décontamination: {_plantule.ResponsableDecontamination}\n" +
                                           $"Statut: {(_plantule.IsActive ? "Actif" : "Inactif")}";

                    BtnModifierPlantule.Visibility = Visibility.Visible;

                    // Charger et afficher l'historique des modifications
                    var plantuleHistory = context.PlantuleHistories
                        .Where(ph => ph.PlantuleId == plantuleId)
                        .OrderBy(ph => ph.DateModified)
                        .ToList();

                    dgPlantuleHistory.ItemsSource = plantuleHistory;
                }
                else
                {
                    txtPlantuleInfo.Text = "Plantule non trouvée.";
                    BtnModifierPlantule.Visibility = Visibility.Collapsed;
                    dgPlantuleHistory.ItemsSource = null;
                }
            }
        }

        private void BtnModifierPlantule_Click(object sender, RoutedEventArgs e)
        {
            if (_plantule == null) return;

            var modificationWindow = new ModifierPlantuleWindow(_plantule);
            modificationWindow.ShowDialog();

            // Refresh details after modification
            BtnRechercherPlantule_Click(sender, e);
        }

        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string ConvertHexToColorName(string hex)
        {
            switch (hex)
            {
                case "#FF008000":
                    return "Vert";
                case "#FFFFFF00":
                    return "Jaune";
                case "#FFFFA500":
                    return "Orange";
                case "#FFFF0000":
                    return "Rouge";
                default:
                    return "Inconnu";
            }
        }
    }
}
