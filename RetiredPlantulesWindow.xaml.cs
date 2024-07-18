using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace InventaireCannabis
{
    public partial class RetiredPlantulesWindow : Window
    {
        public RetiredPlantulesWindow()
        {
            InitializeComponent();
            LoadRetiredPlantules();
        }

        private void LoadRetiredPlantules()
        {
            try
            {
                using (var context = new InventaireCannabisDbContext())
                {
                    var plantules = context.Plantules
                        .Where(p => !p.IsActive)
                        .Select(p => new
                        {
                            p.PlantuleId,
                            p.DateArrivee,
                            p.EtatSante,
                            p.Provenance,
                            p.Description,
                            p.Stade,
                            p.Entreposage,
                            p.Note,
                            p.DateRetrait,
                            QrCodeImage = LoadQrCode(p.QrCode)
                        })
                        .ToList();

                    dataGridRetiredPlantules.ItemsSource = plantules;

                    int retiredPlantulesCount = plantules.Count;
                    Console.WriteLine($"Chargé {retiredPlantulesCount} plantules retirées.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des plantules retirées : {ex.Message}");
                Console.WriteLine($"Erreur lors du chargement des plantules retirées: {ex}");
            }
        }

        private static BitmapImage LoadQrCode(string qrCodeBase64)
        {
            if (string.IsNullOrWhiteSpace(qrCodeBase64))
            {
                return null;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(Convert.FromBase64String(qrCodeBase64));
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du QR Code : {ex.Message}");
                return null;
            }
        }
    }
}
