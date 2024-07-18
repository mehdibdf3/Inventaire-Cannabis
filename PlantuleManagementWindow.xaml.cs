using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InventaireCannabis
{
    public partial class PlantuleManagementWindow : Window
    {
        private string _selectedIdentification;
        private int _identificationCount;

        public PlantuleManagementWindow()
        {
            InitializeComponent();
        }

        private void CbIdentification_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedIdentification = ((System.Windows.Controls.ComboBoxItem)cbIdentification.SelectedItem).Content.ToString();
            using (var context = new InventaireCannabisDbContext())
            {
                _identificationCount = context.Plantules.Count(p => p.PlantuleId.StartsWith(_selectedIdentification)) + 1;
            }
        }

        private void BtnAjouterPlantule_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new InventaireCannabisDbContext())
            {
                var plantuleId = $"{_selectedIdentification}{_identificationCount}";
                var plantule = new Plantule
                {
                    PlantuleId = plantuleId,
                    DateArrivee = dpDateArrivee.SelectedDate ?? DateTime.Now,
                    EtatSante = cbEtatSante.Tag.ToString(), // Use the tag instead of the text
                    Provenance = txtProvenance.Text,
                    Description = txtDescription.Text,
                    Entreposage = cbEntreposage.Text,
                    Note = txtNote.Text,
                    QrCode = GenerateQrCode(plantuleId),
                    IsActive = true,
                    Stade = cbStade.Text
                };

                context.Plantules.Add(plantule);
                context.SaveChanges();

                imgQrCode.Source = LoadQrCode(plantule.QrCode);
                lblPlantuleId.Content = plantuleId;
                qrCodePanel.Visibility = Visibility.Visible;
                btnAjouterPlantule.Visibility = Visibility.Collapsed;
            }
        }

        private string GenerateQrCode(string text)
        {
            using (var generator = new QRCodeGenerator())
            using (var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (var code = new QRCode(data))
            using (var bitmap = code.GetGraphic(20))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private BitmapImage LoadQrCode(string qrCode)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(Convert.FromBase64String(qrCode));
            bitmap.EndInit();
            return bitmap;
        }

        private void BtnImprimerQrCode_Click(object sender, RoutedEventArgs e)
        {
            // Logique d'impression ici
        }

        private void BtnAjouterIdentification_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Nouvelle Identification", "Entrez la nouvelle identification:");
            if (inputDialog.ShowDialog() == true)
            {
                var newIdentification = inputDialog.ResponseText;
                cbIdentification.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = newIdentification });
            }
        }

        private void CbEtatSante_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbEtatSante.SelectedItem != null)
            {
                var selectedTag = ((System.Windows.Controls.ComboBoxItem)cbEtatSante.SelectedItem).Tag.ToString();
                cbEtatSante.Tag = selectedTag;
            }
        }
    }
}
