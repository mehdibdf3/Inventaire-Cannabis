using System.Linq;

namespace InventaireCannabis
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var context = new InventaireCannabisDbContext())
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    var users = new[]
                    {
                        new User { Nom = "Admin", Prenom = "Admin", Email = "admin@admin.com", Password = "Admin1234", Status = "Admin" },
                        new User { Nom = "User", Prenom = "User", Email = "user@example.com", Password = "user", Status = "Employee" }
                    };

                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                if (!context.ResponsablesDecontamination.Any())
                {
                    var responsables = new[]
                    {
                        new ResponsableDecontamination { Nom = "Kadija Houssein Youssouf" },
                        new ResponsableDecontamination { Nom = "Alexandre Tromas" }
                    };

                    context.ResponsablesDecontamination.AddRange(responsables);
                    context.SaveChanges();
                }
            }
        }
    }
}
