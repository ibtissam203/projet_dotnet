using ProductApi.Models;

namespace ProductApi.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Vérifier si la base de données est créée
            await context.Database.EnsureCreatedAsync();

            // Vérifier s'il y a déjà des utilisateurs
            if (!context.Users.Any())
            {
                // Ajouter l'admin
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "System",
                    Email = "admin@beauty.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    IsActive = true,
                    NewsletterSubscription = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Ajouter un client test
                var clientUser = new User
                {
                    FirstName = "Client",
                    LastName = "Test",
                    Email = "client@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("client123"),
                    Role = "Client",
                    IsActive = true,
                    NewsletterSubscription = true,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.AddRange(adminUser, clientUser);
                await context.SaveChangesAsync();
            }

            // Vérifier s'il y a déjà des produits
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "rouge a levres glossy glaze essence",
                        Price = 49.99m,
                        Quantity = 25,
                        ImageUrl = "/images/products/rouge-a-levres-glossy-glaze-essence.jpg",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "super fine liner pen 01 essence",
                        Price = 79.99m,
                        Quantity = 15,
                        ImageUrl = "/images/products/super-fine-liner-pen-01-essence.jpg",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "baume teinte levres joues juicy berry essence",
                        Price = 29.99m,
                        Quantity = 8, // Stock faible
                        ImageUrl = "/images/products/baume-teinte-levres-joues-juicy-berry-essence.jpg",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Name = "bronzeur stick baby got bronze 10 essence",
                        Price = 19.99m,
                        Quantity = 0, // Rupture de stock
                        ImageUrl = "/images/products/bronzeur-stick-baby-got-bronze-10-essence.jpg",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}