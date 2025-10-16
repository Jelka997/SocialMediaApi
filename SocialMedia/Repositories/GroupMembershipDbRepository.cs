using Microsoft.Data.Sqlite;
using SocialMedia.Domain;

namespace SocialMedia.Repositories;

public class GroupMembershipDbRepository
{
    private readonly string connectionString;

    public GroupMembershipDbRepository(IConfiguration configuration)
    {
        connectionString = configuration["ConnectionString:SQLiteConnection"];
    }

    public bool AddUser(int groupId, int userId)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open(); 
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = @"
            SELECT COUNT(*) 
            FROM GroupMemberships 
            WHERE UserId = @UserId AND GroupId = @GroupId";
            checkCommand.Parameters.AddWithValue("@UserId", userId);
            checkCommand.Parameters.AddWithValue("@GroupId", groupId);

            long count = (long)checkCommand.ExecuteScalar();

            if (count > 0)
            {
                return false;
            }

            string query = "INSERT INTO GroupMemberships (GroupId, UserId) VALUES (@GroupId, @UserId); SELECT LAST_INSERT_ROWID();";
            using SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@GroupId", groupId);
            command.Parameters.AddWithValue("@UserId", userId);

            int affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;

        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri povezivanju sa bazom ili izvršavanju SQL upita: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
}