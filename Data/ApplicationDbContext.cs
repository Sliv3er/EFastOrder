using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EFastOrder.Models;

namespace EFastOrder.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<PanierItem> PanierItems { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandeItem> CommandeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuration des relations
            builder.Entity<Article>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PanierItem>()
                .HasOne(pi => pi.Panier)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PanierId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PanierItem>()
                .HasOne(pi => pi.Article)
                .WithMany()
                .HasForeignKey(pi => pi.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommandeItem>()
                .HasOne(ci => ci.Commande)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CommandeItem>()
                .HasOne(ci => ci.Article)
                .WithMany()
                .HasForeignKey(ci => ci.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data - Catégories
            builder.Entity<Categorie>().HasData(
                new Categorie { Id = 1, Nom = "Entrées", Description = "Nos entrées fraîches et savoureuses", Icone = "fas fa-leaf" },
                new Categorie { Id = 2, Nom = "Plats Principaux", Description = "Nos plats signatures préparés avec soin", Icone = "fas fa-utensils" },
                new Categorie { Id = 3, Nom = "Pizzas", Description = "Pizzas artisanales cuites au feu de bois", Icone = "fas fa-pizza-slice" },
                new Categorie { Id = 4, Nom = "Burgers", Description = "Burgers gourmets avec des ingrédients frais", Icone = "fas fa-hamburger" },
                new Categorie { Id = 5, Nom = "Desserts", Description = "Douceurs sucrées pour finir en beauté", Icone = "fas fa-ice-cream" },
                new Categorie { Id = 6, Nom = "Boissons", Description = "Boissons fraîches et chaudes", Icone = "fas fa-glass-cheers" }
            );

            // Seed data - Articles (Images via Unsplash)
            builder.Entity<Article>().HasData(
                // Entrées
                new Article { Id = 1, Nom = "Salade César", Description = "Laitue romaine, croûtons, parmesan, sauce César maison", Prix = 12.00m, CategorieId = 1, IsDisponible = true, TempPreparation = 10, ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=400&h=300&fit=crop" },
                new Article { Id = 2, Nom = "Soupe à l'oignon", Description = "Soupe traditionnelle gratinée au fromage", Prix = 9.50m, CategorieId = 1, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400&h=300&fit=crop" },
                new Article { Id = 3, Nom = "Bruschetta", Description = "Pain grillé, tomates fraîches, basilic, huile d'olive", Prix = 8.00m, CategorieId = 1, IsDisponible = true, TempPreparation = 8, ImageUrl = "https://images.unsplash.com/photo-1572695157366-5e585ab2b69f?w=400&h=300&fit=crop" },

                // Plats Principaux
                new Article { Id = 4, Nom = "Steak Frites", Description = "Steak de bœuf, frites maison, sauce au poivre", Prix = 28.00m, CategorieId = 2, IsDisponible = true, TempPreparation = 25, ImageUrl = "https://images.unsplash.com/photo-1600891964092-4316c288032e?w=400&h=300&fit=crop" },
                new Article { Id = 5, Nom = "Saumon Grillé", Description = "Filet de saumon, légumes de saison, riz basmati", Prix = 32.00m, CategorieId = 2, IsDisponible = true, TempPreparation = 20, ImageUrl = "https://images.unsplash.com/photo-1467003909585-2f8a72700288?w=400&h=300&fit=crop" },
                new Article { Id = 6, Nom = "Poulet Rôti", Description = "Poulet fermier, pommes de terre rôties, jus de cuisson", Prix = 22.00m, CategorieId = 2, IsDisponible = true, TempPreparation = 30, ImageUrl = "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=400&h=300&fit=crop" },
                new Article { Id = 7, Nom = "Pâtes Carbonara", Description = "Spaghetti, lardons, crème, parmesan, œuf", Prix = 18.00m, CategorieId = 2, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1612874742237-6526221588e3?w=400&h=300&fit=crop" },

                // Pizzas
                new Article { Id = 8, Nom = "Pizza Margherita", Description = "Sauce tomate, mozzarella, basilic frais", Prix = 15.00m, CategorieId = 3, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400&h=300&fit=crop" },
                new Article { Id = 9, Nom = "Pizza 4 Fromages", Description = "Mozzarella, gorgonzola, chèvre, parmesan", Prix = 19.00m, CategorieId = 3, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=400&h=300&fit=crop" },
                new Article { Id = 10, Nom = "Pizza Pepperoni", Description = "Sauce tomate, mozzarella, pepperoni épicé", Prix = 17.00m, CategorieId = 3, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=400&h=300&fit=crop" },

                // Burgers
                new Article { Id = 11, Nom = "Classic Burger", Description = "Bœuf, cheddar, salade, tomate, oignon, sauce maison", Prix = 16.00m, CategorieId = 4, IsDisponible = true, TempPreparation = 18, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400&h=300&fit=crop" },
                new Article { Id = 12, Nom = "Chicken Burger", Description = "Poulet pané, avocat, salade, sauce ranch", Prix = 14.50m, CategorieId = 4, IsDisponible = true, TempPreparation = 18, ImageUrl = "https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=400&h=300&fit=crop" },
                new Article { Id = 13, Nom = "Veggie Burger", Description = "Steak végétal, crudités, sauce vegan", Prix = 13.00m, CategorieId = 4, IsDisponible = true, TempPreparation = 15, ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595e6cdc07a?w=400&h=300&fit=crop" },

                // Desserts
                new Article { Id = 14, Nom = "Tiramisu", Description = "Mascarpone, café, cacao, biscuits", Prix = 10.00m, CategorieId = 5, IsDisponible = true, TempPreparation = 5, ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400&h=300&fit=crop" },
                new Article { Id = 15, Nom = "Crème Brûlée", Description = "Crème vanille, caramel croustillant", Prix = 9.00m, CategorieId = 5, IsDisponible = true, TempPreparation = 5, ImageUrl = "https://images.unsplash.com/photo-1470124182917-cc6e71b22ecc?w=400&h=300&fit=crop" },
                new Article { Id = 16, Nom = "Fondant au Chocolat", Description = "Cœur coulant, glace vanille", Prix = 11.00m, CategorieId = 5, IsDisponible = true, TempPreparation = 12, ImageUrl = "https://images.unsplash.com/photo-1606313564200-e75d5e30476c?w=400&h=300&fit=crop" },

                // Boissons
                new Article { Id = 17, Nom = "Coca-Cola", Description = "33cl", Prix = 3.50m, CategorieId = 6, IsDisponible = true, TempPreparation = 1, ImageUrl = "https://images.unsplash.com/photo-1554866585-cd94860890b7?w=400&h=300&fit=crop" },
                new Article { Id = 18, Nom = "Jus d'Orange Pressé", Description = "Oranges fraîches pressées", Prix = 5.00m, CategorieId = 6, IsDisponible = true, TempPreparation = 3, ImageUrl = "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=400&h=300&fit=crop" },
                new Article { Id = 19, Nom = "Eau Minérale", Description = "Safia 50cl", Prix = 1.50m, CategorieId = 6, IsDisponible = true, TempPreparation = 1, ImageUrl = "https://images.unsplash.com/photo-1548839140-29a749e1cf4d?w=400&h=300&fit=crop" },
                new Article { Id = 20, Nom = "Café Espresso", Description = "Café traditionnel", Prix = 3.00m, CategorieId = 6, IsDisponible = true, TempPreparation = 3, ImageUrl = "https://images.unsplash.com/photo-1510707577719-ae7c14805e3a?w=400&h=300&fit=crop" }
            );
        }
    }
}
