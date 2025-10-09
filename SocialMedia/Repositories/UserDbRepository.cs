using Microsoft.Data.Sqlite;
using SocialMedia.Domain;

namespace SocialMedia.Repositories
{
    public class UserDbRepository
    {
        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/socialdata.db");
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    string Username = reader["Username"].ToString();
                    string Name = reader["Name"].ToString();
                    string LastName = reader["Surname"].ToString();
                    DateTime Birthday = DateTime.Parse(reader["Birthday"].ToString());
                    User user = new User(Id, Username, Name, LastName, Birthday);
                    users.Add(user);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return users;
        }

        public User GetByIdDb(int id)
        {
            User user = null;
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/socialdata.db");
                connection.Open();

                string query = "SELECT * FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    string Username = reader["Username"].ToString();
                    string Name = reader["Name"].ToString();
                    string LastName = reader["Surname"].ToString();
                    DateTime Birthday = DateTime.Parse(reader["Birthday"].ToString());
                    return new User(Id, Username, Name, LastName, Birthday);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return user;
        }

        public User CreateNewUser(User newUser)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/socialdata.db");
                connection.Open();
                string query = "INSERT INTO Users(Username, Name, Surname, Birthday) VALUES(@Username, @Name, @Surname, @Birthday)";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", newUser.Username);
                command.Parameters.AddWithValue("@Name", newUser.Name);
                command.Parameters.AddWithValue("@Surname", newUser.LastName);
                command.Parameters.AddWithValue("@Birthday", newUser.Birthday);

                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return newUser;
        }

        public User UpdateUser(int id, User user)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/socialdata.db");
                connection.Open();
                string query = "UPDATE Users SET Username=@Username, Name = @Name, Surname = @Surname, Birthday = @Birthday WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id",id);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.LastName);
                command.Parameters.AddWithValue("@Birthday", user.Birthday);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0) { return null; }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return user;
        }
        public int DeleteUser(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/socialdata.db");
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return 0;
        }
    }
}