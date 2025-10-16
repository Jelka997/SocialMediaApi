using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SocialMedia.Domain;

public class User
{
    //Korisnik ima id, korisničko ime (string), ime (string), prezime (string) i datum rođenja (DateTime).
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    [JsonIgnore]
    public List<Group>? Groups { get; set; }
    public List<Post> Posts { get; set; }

    public User(int id, string username, string name, string lastName, DateTime birthday)
    {
        Id = id;
        Username = username;
        Name = name;
        LastName = lastName;
        Birthday = birthday;
        Groups = new List<Group>();
        Posts = new List<Post>();
    }

    public User()
    {
        Groups = new List<Group>();
    }

    public string FileFormat()
    {
        return $"{Id},{Username},{Name},{LastName},{Birthday.ToString("yyyy-MM-dd")}";
    }
}