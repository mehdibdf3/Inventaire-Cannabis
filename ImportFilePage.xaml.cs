using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using OfficeOpenXml;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace InventaireCannabis
{
    public partial class ImportFilePage : Window
    {
        private readonly InventaireCannabisDbContext _dbContext;

        public ImportFilePage()
        {
            InitializeComponent();
            _dbContext = new InventaireCannabisDbContext();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                LoadDataFromExcel(openFileDialog.FileName);
            }
        }

        private void LoadDataFromExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string identification = worksheet.Cells[row, 1].Text;
                    string etatSante = worksheet.Cells[row, 2].Text;

                    DateTime dateArrivee;
                    if (!DateTime.TryParseExact(worksheet.Cells[row, 3].Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateArrivee))
                    {
                        dateArrivee = DateTime.MinValue;
                    }

                    string provenance = worksheet.Cells[row, 4].Text;
                    string description = worksheet.Cells[row, 5].Text;
                    string stade = worksheet.Cells[row, 6].Text;
                    string entreposage = worksheet.Cells[row, 7].Text;
                    bool actif = worksheet.Cells[row, 8].Text == "1";

                    DateTime? dateRetrait = null;
                    if (!string.IsNullOrEmpty(worksheet.Cells[row, 9].Text))
                    {
                        DateTime tempDateRetrait;
                        if (DateTime.TryParseExact(worksheet.Cells[row, 9].Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out tempDateRetrait))
                        {
                            dateRetrait = tempDateRetrait;
                        }
                    }

                    string raisonRetrait = worksheet.Cells[row, 10].Text;
                    string responsable = worksheet.Cells[row, 11].Text;
                    string note = worksheet.Cells[row, 12].Text;

                    EnregistrerPlantule(identification, etatSante, dateArrivee, provenance, description, stade, entreposage, actif, dateRetrait, raisonRetrait, responsable, note);
                }
            }

            MessageBox.Show("Importation terminée avec succès !");
            ActualiserInventaire();
        }

        private void EnregistrerPlantule(string identification, string etatSante, DateTime dateArrivee, string provenance, string description, string stade, string entreposage, bool actif, DateTime? dateRetrait, string raisonRetrait, string responsable, string note)
        {
            try
            {
                var plantule = new Plantule
                {
                    PlantuleId = Guid.NewGuid().ToString(),
                    EtatSante = etatSante,
                    DateArrivee = dateArrivee,
                    Provenance = provenance,
                    Description = description,
                    Stade = stade,
                    Entreposage = entreposage,
                    IsActive = actif,
                    DateRetrait = dateRetrait,
                    ResponsableDecontamination = responsable,
                    Note = note,
                    QrCode = GenerateQrCode(identification)
                };

                _dbContext.Plantules.Add(plantule);
                _dbContext.SaveChanges();
                Console.WriteLine($"Plantule {identification} enregistrée avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement de la plantule : {ex.Message}");
                Console.WriteLine($"Erreur lors de l'enregistrement de la plantule {identification}: {ex}");
            }
        }

        private string GenerateQrCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            using (MemoryStream memory = new MemoryStream())
            {
                qrCodeImage.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    bitmapImage.StreamSource.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void ActualiserInventaire()
        {
            var inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
        }
    }
}
