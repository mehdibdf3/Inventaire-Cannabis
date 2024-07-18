using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace InventaireCannabis
{
    public class InventaireCannabisDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Plantule> Plantules { get; set; }
        public DbSet<ResponsableDecontamination> ResponsablesDecontamination { get; set; }
        public DbSet<PlantuleHistory> PlantuleHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=InventaireCannabis.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Nom = "Admin", Prenom = "Admin", Email = "admin@admin.com", Password = "Admin1234", Status = "Admin" }
            );
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }

    public class Plantule
    {
        [Key]
        public int Id { get; set; }
        public string PlantuleId { get; set; }
        public DateTime DateArrivee { get; set; }
        public string EtatSante { get; set; }
        public string Provenance { get; set; }
        public string Description { get; set; }
        public string Stade { get; set; }
        public string Entreposage { get; set; }
        public string Note { get; set; }
        public string QrCode { get; set; }
        public bool IsActive { get; set; }
        public string ResponsableDecontamination { get; set; }
        public DateTime? DateRetrait { get; set; }

        [NotMapped]
        public BitmapImage QrCodeImage { get; set; }
    }

    public class ResponsableDecontamination
    {
        public int Id { get; set; }
        public string Nom { get; set; }
    }

    public class PlantuleHistory
    {
        [Key]
        public int Id { get; set; }
        public string PlantuleId { get; set; }
        public DateTime DateModified { get; set; }
        public string EtatSante { get; set; }
        public string Provenance { get; set; }
        public string Description { get; set; }
        public string Entreposage { get; set; }
        public string Note { get; set; }
        public string Stade { get; set; }
        public string ResponsableDecontamination { get; set; }
        public bool IsActive { get; set; }
    }
}
