using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {   private GroupRepository groupRepository = new GroupRepository();
        private UserRepository userRepository = new UserRepository();


        [HttpGet]
        public ActionResult<List<User>> GetAll()
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
                    User user = new User(Id,Username, Name, LastName, Birthday);
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

            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(UserRepository.Data[id]);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.LastName) || string.IsNullOrWhiteSpace(newUser.Birthday.ToString()))
            {
                return BadRequest();
            }

            newUser.Id = SracunajNoviId(UserRepository.Data.Keys.ToList());
            UserRepository.Data[newUser.Id] = newUser;
            userRepository.SaveData();

            return Ok(newUser);
        }


        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (string.IsNullOrWhiteSpace(uUser.Username) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.LastName) || string.IsNullOrWhiteSpace(uUser.Birthday.ToString()))
            {
                return BadRequest();
            }
            if (!UserRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            User user = UserRepository.Data[id];
            user.Username = uUser.Username;
            user.Name = uUser.Name;
            user.LastName = uUser.LastName;
            user.Birthday = uUser.Birthday;
            userRepository.SaveData();

            return Ok(user);
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
}
