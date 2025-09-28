using SocialMedia.Domain;

namespace SocialMedia.Repositories;

public class UserRepository
{
    private const string filePath = "Data/korisnici.csv";
    public static Dictionary<int, User> Data;

    public UserRepository()
    {
        if (Data == null)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        Data = new Dictionary<int, User>();
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string username = attributes[1];
                string name = attributes[2];
                string lastName = attributes[3];
                DateTime dateCreated = DateTime.Parse(attributes[4]);
                User user = new User(id, username, name, lastName, dateCreated);
                Data.Add(id, user);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Greska: {e.Message}");
        }
    }

    private void SaveData()
    {
        List<string> lines = new List<string>();
        foreach (User user in Data.Values)
        {
            lines.Add(user.FileFormat());
        }
        File.WriteAllLines(filePath, lines);
    }
    
}