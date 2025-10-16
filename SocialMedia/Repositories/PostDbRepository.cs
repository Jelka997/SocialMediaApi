using Microsoft.Data.Sqlite;
using SocialMedia.Domain;

namespace SocialMedia.Repositories
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetAll()
        {
            Dictionary<int, User> users = new Dictionary<int, User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                using SqliteCommand command = new SqliteCommand(@"SELECT u.Id, u.Username, u.Name, u.Surname, u.Birthday, p.Id AS PostId, p.Content, p.Date AS PostDate FROM Users u LEFT JOIN Posts p ON u.Id = p.UserId ORDER BY u.Id;", connection);
                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["Id"]);

                    if (!users.ContainsKey(userId))
                    {
                        users[userId] = new User
                        {
                            Id = userId,
                            Username = Convert.ToString(reader["Username"]),
                            Name = Convert.ToString(reader["Name"]),
                            LastName = Convert.ToString(reader["Username"]),
                            Birthday = Convert.ToDateTime(reader["Birthday"]),
                            Posts = new List<Post>()

                        };
                    }
                    if (reader["PostId"] != DBNull.Value)
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["PostId"]),
                            Content = Convert.ToString(reader["Content"]),
                            Date = Convert.ToDateTime(reader["PostDate"]),
                            UserId = userId
                        };
                        users[userId].Posts.Add(post);
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
            return users.Values.ToList();
        }
    } 
}
