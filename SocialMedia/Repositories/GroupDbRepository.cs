using Microsoft.Data.Sqlite;
using SocialMedia.Domain;

namespace SocialMedia.Repositories;

public class GroupDbRepository
{
    private readonly string connectionString;

    public GroupDbRepository(IConfiguration configuration)
    {
        connectionString = configuration["ConnectionString:SQLiteConnection"];
    }
    public List<Group> GetAll()
    {
        List<Group> groups = new List<Group>();
        try
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
        
            string query = "SELECT * FROM Groups";
            using SqliteCommand command = new SqliteCommand(query, connection);

            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["Id"]);
                string name = reader["Name"].ToString();
                DateTime dateCreated = DateTime.Parse(reader["CreationDate"].ToString());
                Group group = new Group(id, name, dateCreated);
                groups.Add(group);
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
        return groups;
    }

    public Group GetById(int id)
    {
        Group group = null;
        try
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
        
            string query = "SELECT * FROM Groups  WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                group = new Group(Convert.ToInt32(reader["Id"]), reader["Name"].ToString(), DateTime.Parse(reader["CreationDate"].ToString()));
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

        if (group == null)
        {
            Console.WriteLine($"Grupa koja sadrži traženi ID {id} nije pronađena");
        }
        return group;
    }
    
    //Dodati Create, koji prihvata instancu Group klase, smešta je u bazu i vraća Group objekat koji ima nov id,
    public Group Create(Group group)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection("Data Source=database/socialdata.db");
            connection.Open();
        
            string query = "INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @CreationDate); SELECT LAST_INSERT_ROWID();";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", group.Name);
            command.Parameters.AddWithValue("@CreationDate", group.DateCreated);
            
            group.Id = Convert.ToInt32(command.ExecuteScalar());
            return group;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri povezivanju sa bazom ili izvršavanju SQL upita: {ex.Message}");
            throw;
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Greška u formatu podataka: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Greška jer konekcija nije ili je više puta otvorena: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
    
    //Dodati Update, koji prihvata instancu Group klase i ažurira postojeću grupu u bazi,
    public Group Update(Group group)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection("Data Source=database/socialdata.db");
            connection.Open();
        
            string query = "UPDATE Groups SET Name=@Name, CreationDate=@CreationDate WHERE Id=@Id;";
            using SqliteCommand command = new SqliteCommand(query, connection);
            
            command.Parameters.AddWithValue("@Id", group.Id);
            command.Parameters.AddWithValue("@Name", group.Name);
            command.Parameters.AddWithValue("@CreationDate", group.DateCreated);
            
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0 ? group : null;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri povezivanju sa bazom ili izvršavanju SQL upita: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Greška jer konekcija nije ili je više puta otvorena: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
    
    // Dodati Delete, koji prihvata id i briše grupu pod datim identifikatorom u bazi,
    public bool Delete(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection("Data Source=database/socialdata.db");
            connection.Open();
        
            string query = "DELETE FROM Groups WHERE Id=@Id;";
            using SqliteCommand command = new SqliteCommand(query, connection);
            
            command.Parameters.AddWithValue("@Id", id);
            
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri povezivanju sa bazom ili izvršavanju SQL upita: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Greška jer konekcija nije ili je više puta otvorena: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
}