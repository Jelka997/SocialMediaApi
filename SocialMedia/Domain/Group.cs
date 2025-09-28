namespace SocialMedia.Domain;

public class Group
{
    //Grupa ima id, ime (string) i datum osnivanja (DateTime).
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public List<User> Users { get; set; }

    public Group(int id, string name, DateTime dateCreated)
    {
        Id = id;
        Name = name;
        DateCreated = dateCreated;
        Users = new List<User>();
    }

    public string FileFormat()
    {
        return $"{Id},{Name},{DateCreated.ToString("yyyy-MM-dd")}";
    }
    
}