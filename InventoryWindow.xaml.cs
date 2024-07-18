using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows.Media.Imaging;
using System;

namespace InventaireCannabis
{
    public partial class InventoryWindow : Window
    {
        private int MaxCapacity;

        public InventoryWindow()
        {
            InitializeComponent();
            LoadMaxCapacity();
            LoadInventory();
        }

        private void LoadMaxCapacity()
        {
            MaxCapacity = Properties.Settings.Default.MaxCapacity;
        }

        private void SaveMaxCapacity()
        {
            Properties.Settings.Default.MaxCapacity = MaxCapacity;
            Properties.Settings.Default.Save();
        }

        private void LoadInventory()
        {
            try
            {
                using (var context = new InventaireCannabisDbContext())
                {
                    var plantules = context.Plantules
                        .Where(p => p.IsActive)
                        .ToList();

                    foreach (var plantule in plantules)
                    {
                        plantule.QrCodeImage = LoadQrCode(plantule.QrCode);
                    }

                    dgInventory.ItemsSource = plantules;

                    int plantuleCount = plantules.Count;
                    int remainingCapacity = MaxCapacity - plantuleCount;

                    txtCount.Text = $"{plantuleCount} / {MaxCapacity}";
                    txtRemainingCapacity.Text = $"Capacité restante : {remainingCapacity}";

                    Console.WriteLine($"Chargé {plantuleCount} plantules actives.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'inventaire : {ex.Message}");
                Console.WriteLine($"Erreur lors du chargement de l'inventaire: {ex}");
            }
        }

        private static BitmapImage LoadQrCode(string qrCodeBase64)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(Convert.FromBase64String(qrCodeBase64));
            bitmap.EndInit();
            return bitmap;
        }

        private void BtnModifyCapacity_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Modifier Capacité Max", "Entrez la nouvelle capacité maximale:");
            if (inputDialog.ShowDialog() == true)
            {
                if (int.TryParse(inputDialog.ResponseText, out int newMaxCapacity) && newMaxCapacity >= 0)
                {
                    MaxCapacity = newMaxCapacity;
                    SaveMaxCapacity();
                    LoadInventory(); // Reload to update UI
                }
                else
                {
                    MessageBox.Show("Veuillez entrer un nombre valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
