using System.Text.RegularExpressions;

namespace SocialMedia.Domain;

public class User
{
    //Korisnik ima id, korisničko ime (string), ime (string), prezime (string) i datum rođenja (DateTime). Grupa ima id, ime (string) i datum osnivanja (DateTime)
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public List<Group> Groups { get; set; }

    public User(int id, string username, string name, string lastName, DateTime birthday)
    {
        Id = id;
        Username = username;
        Name = name;
        LastName = lastName;
        Birthday = birthday;
        Groups = new List<Group>();
    }
}