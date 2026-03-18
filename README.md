# 🔥 E-FastOrder

> Application web de commande de restaurant en ligne — Projet ASP.NET MVC

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

---

## 📋 Description

**E-FastOrder** est une plateforme de commande de restaurant en ligne developpee avec ASP.NET MVC (.NET 8). L'application permet aux clients de parcourir un menu, ajouter des articles au panier, passer des commandes, et suivre leur historique. Un panneau d'administration permet de gerer les articles, categories et commandes.

### Technologies utilisees

| Composant | Technologie |
|-----------|------------|
| Framework | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| ADO.NET | CRUD Categories (sans ORM) |
| Base de donnees | SQLite |
| Authentification | ASP.NET Identity |
| Frontend | Bootstrap 5, Font Awesome 6, CSS custom |
| Images | Unsplash API |

---

## ✨ Fonctionnalites

### Client
- 🍽️ **Menu interactif** — Parcourir les plats avec images, filtrer par categorie, rechercher
- 🛒 **Panier** — Ajouter/retirer des articles, modifier les quantites
- 📦 **Commandes** — Passer une commande, consulter l'historique, annuler une commande en attente
- 🔐 **Authentification** — Inscription, connexion, gestion du profil

### Administration
- 📊 **Tableau de bord** — Statistiques (articles, categories, commandes, revenu)
- 🍕 **Gestion des Articles** — CRUD complet avec **Entity Framework**
- 🏷️ **Gestion des Categories** — CRUD complet avec **ADO.NET** (exigence projet)
- 📋 **Gestion des Commandes** — Voir toutes les commandes, changer le statut

---

## 🏗️ Architecture du Projet

```
EFastOrder/
├── Controllers/
│   ├── HomeController.cs          # Menu, recherche, filtrage
│   ├── ArticleController.cs       # CRUD Articles (Entity Framework)
│   ├── CategorieController.cs     # CRUD Categories (ADO.NET)
│   ├── PanierController.cs        # Gestion du panier
│   ├── CommandeController.cs      # Commandes client + admin
│   ├── AccountController.cs       # Authentification (Identity)
│   └── DashboardController.cs     # Tableau de bord admin
├── Models/
│   ├── Article.cs                 # Entite article
│   ├── Categorie.cs               # Entite categorie
│   ├── Commande.cs                # Entite commande + items
│   ├── Panier.cs                  # Entite panier + items
│   └── ViewModels.cs              # ViewModels
├── Data/
│   ├── ApplicationDbContext.cs    # DbContext EF Core + seed data
│   └── CategorieAdoRepository.cs  # Repository ADO.NET
├── Views/                         # Vues Razor (.cshtml)
├── wwwroot/css/site.css           # Theme custom (dark mode)
└── Program.cs                     # Configuration de l'app
```

---

## 🚀 Installation & Lancement

### Prerequis
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Etapes

```bash
# 1. Cloner le projet
git clone https://github.com/Sliv3er/EFastOrder.git
cd EFastOrder

# 2. Restaurer les packages
dotnet restore

# 3. Lancer l'application
dotnet run --urls "http://localhost:5000"
```

L'application sera accessible sur **http://localhost:5000**

> La base de donnees SQLite est creee automatiquement au premier lancement avec des donnees de demonstration.

---

## 🔑 Comptes de test

| Role | Email | Mot de passe |
|------|-------|-------------|
| **Admin** | `admin@efastorder.com` | `Admin123` |
| **Client** | Creer un compte via l'inscription | Min. 4 caracteres |

---

## 📦 Exigences du Projet

| Exigence | Implementation | Statut |
|----------|---------------|--------|
| API web ASP.NET MVC | Architecture MVC complete avec 7 controllers | ✅ |
| Entity Framework | CRUD Articles, Commandes, Panier | ✅ |
| ADO.NET (min. 1 CRUD) | CRUD Categories via `CategorieAdoRepository` | ✅ |
| Integration de template | Bootstrap 5 + CSS custom (dark theme) | ✅ |
| Authentification (bonus) | ASP.NET Identity avec roles Admin/Client | ✅ |

---

## 🛠️ Stack Technique

- **Backend** : ASP.NET Core MVC 8.0
- **ORM** : Entity Framework Core (SQLite)
- **ADO.NET** : Microsoft.Data.Sqlite
- **Auth** : ASP.NET Core Identity
- **Frontend** : Bootstrap 5.3, Font Awesome 6, Animate.css, Google Fonts (Poppins)
- **Images** : Unsplash (photos haute qualite)
- **BDD** : SQLite (fichier local)

---

## 📄 Licence

Projet academique — Developpement .NET
