using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace InventaireCannabis
{
    public partial class ModifierPlantuleWindow : Window
    {
        private Plantule _plantule;

        public ModifierPlantuleWindow(Plantule plantule)
        {
            InitializeComponent();
            _plantule = plantule;
            LoadPlantuleDetails();
            LoadResponsables();
        }

        private void LoadPlantuleDetails()
        {
            if (IsValidColor(_plantule.EtatSante))
            {
                var color = (Color)ColorConverter.ConvertFromString(_plantule.EtatSante);
                cbEtatSante.SelectedItem = new SolidColorBrush(color);
            }
            else
            {
                MessageBox.Show("La valeur de l'état de santé n'est pas une couleur valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            cbEntreposage.Text = _plantule.Entreposage;
            cbResponsableDecontamination.Text = _plantule.ResponsableDecontamination; // Charger le responsable existant
        }

        private void LoadResponsables()
        {
            cbResponsableDecontamination.Items.Clear();
            using (var context = new InventaireCannabisDbContext())
            {
                var responsables = context.ResponsablesDecontamination.Select(r => r.Nom).ToList();
                foreach (var responsable in responsables)
                {
                    cbResponsableDecontamination.Items.Add(responsable);
                }
            }
        }

        private void BtnAjouterNouveauResponsable_Click(object sender, RoutedEventArgs e)
        {
            var window = new NouveauResponsableWindow();
            if (window.ShowDialog() == true)
            {
                LoadResponsables();
            }
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var plantule = context.Plantules.Find(_plantule.Id);
                if (plantule != null)
                {
                    // Créer une nouvelle entrée dans l'historique avant de modifier
                    var plantuleHistory = new PlantuleHistory
                    {
                        PlantuleId = plantule.PlantuleId,
                        DateModified = DateTime.Now,
                        EtatSante = plantule.EtatSante,
                        Provenance = plantule.Provenance,
                        Description = plantule.Description,
                        Entreposage = plantule.Entreposage,
                        Note = plantule.Note,
                        Stade = plantule.Stade,
                        ResponsableDecontamination = plantule.ResponsableDecontamination,
                        IsActive = plantule.IsActive
                    };
                    context.PlantuleHistories.Add(plantuleHistory);

                    // Mettre à jour les détails de la plantule
                    if (cbEtatSante.SelectedItem is SolidColorBrush selectedBrush)
                    {
                        plantule.EtatSante = selectedBrush.Color.ToString();
                    }
                    plantule.Entreposage = cbEntreposage.Text;
                    plantule.ResponsableDecontamination = cbResponsableDecontamination.Text; // Enregistrer le responsable

                    if (dpDateRetrait.SelectedDate.HasValue)
                    {
                        plantule.DateRetrait = dpDateRetrait.SelectedDate.Value;
                        plantule.IsActive = false;
                    }

                    context.SaveChanges();
                }
            }

            Close();
        }

        private bool IsValidColor(string colorString)
        {
            try
            {
                ColorConverter.ConvertFromString(colorString);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
