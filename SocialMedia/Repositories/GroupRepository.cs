using SocialMedia.Domain;

namespace SocialMedia.Repositories;

public class GroupRepository
{
    private string filePath = "Data/grupe.csv";
    public static Dictionary<int, Group> Data;

    public GroupRepository()
    {
        if (Data == null)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        try
        {
            Data = new Dictionary<int, Group>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string name = attributes[1];
                DateTime dateCreated = DateTime.Parse(attributes[2]);
                Group group = new Group(id, name, dateCreated);
                Data.Add(id, group);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Greska: {e.Message}");
        }
    }

    public void SaveData()
    {
        List<string> lines = new List<string>();
        foreach (Group group in  Data.Values )
        {
            lines.Add(group.FileFormat());
        }
        File.WriteAllLines(filePath, lines);
    }

}