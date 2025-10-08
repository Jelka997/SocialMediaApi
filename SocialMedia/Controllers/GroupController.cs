using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController : ControllerBase
{
    private GroupRepository groupRepository = new GroupRepository();
    private UserRepository userRepository = new UserRepository();
    
    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        List<Group> groups = new List<Group>();
        try
        {
            using SqliteConnection connection = new SqliteConnection("Data Source=database/socialdata.db");
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
        return Ok(groups);
    }
    
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        if (string.IsNullOrWhiteSpace(newGroup.Name)  || string.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
        {
            return BadRequest();
        }

        newGroup.Id = SracunajNoviId(GroupRepository.Data.Keys.ToList());
        GroupRepository.Data[newGroup.Id] = newGroup;
        groupRepository.SaveData();

        return Ok(newGroup);
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (!GroupRepository.Data.ContainsKey(id))
        {
            return NotFound();
        }

        GroupRepository.Data.Remove(id);
        groupRepository.SaveData();

        return NoContent();
    }
    
    private int SracunajNoviId(List<int> identifikatori)
    {
        int maxId = 0;
        foreach (int id in identifikatori)
        {
            if (id > maxId)
            {
                maxId = id;
            }
        }

        return maxId + 1;
    }
}
