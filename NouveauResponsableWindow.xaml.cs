using System.Windows;

namespace InventaireCannabis
{
    public partial class NouveauResponsableWindow : Window
    {
        public NouveauResponsableWindow()
        {
            InitializeComponent();
        }

        private void BtnAjouterResponsable_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var responsable = new ResponsableDecontamination
                {
                    Nom = txtNomResponsable.Text
                };

                context.ResponsablesDecontamination.Add(responsable);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
