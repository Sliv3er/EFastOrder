using Microsoft.Data.Sqlite;
using EFastOrder.Models;

namespace EFastOrder.Data
{
    /// <summary>
    /// Repository ADO.NET pour la gestion des catégories.
    /// Implémentation CRUD complète avec ADO.NET pur (sans Entity Framework)
    /// conformément aux exigences du projet.
    /// </summary>
    public class CategorieAdoRepository
    {
        private readonly string _connectionString;

        public CategorieAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Récupérer toutes les catégories
        /// </summary>
        public List<Categorie> GetAll()
        {
            var categories = new List<Categorie>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Nom, Description, Icone FROM Categories ORDER BY Nom";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Categorie
                        {
                            Id = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Icone = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Récupérer une catégorie par son Id
        /// </summary>
        public Categorie? GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Nom, Description, Icone FROM Categories WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Categorie
                        {
                            Id = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Icone = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Créer une nouvelle catégorie (INSERT)
        /// </summary>
        public int Create(Categorie categorie)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Categories (Nom, Description, Icone) 
                    VALUES (@Nom, @Description, @Icone);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@Nom", categorie.Nom);
                command.Parameters.AddWithValue("@Description", (object?)categorie.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Icone", (object?)categorie.Icone ?? DBNull.Value);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Mettre à jour une catégorie (UPDATE)
        /// </summary>
        public bool Update(Categorie categorie)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Categories 
                    SET Nom = @Nom, Description = @Description, Icone = @Icone 
                    WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", categorie.Id);
                command.Parameters.AddWithValue("@Nom", categorie.Nom);
                command.Parameters.AddWithValue("@Description", (object?)categorie.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Icone", (object?)categorie.Icone ?? DBNull.Value);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Supprimer une catégorie (DELETE)
        /// </summary>
        public bool Delete(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Categories WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Vérifier si une catégorie a des articles associés
        /// </summary>
        public bool HasArticles(int categorieId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Articles WHERE CategorieId = @CategorieId";
                command.Parameters.AddWithValue("@CategorieId", categorieId);

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Compter le nombre total de catégories
        /// </summary>
        public int Count()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Categories";

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
