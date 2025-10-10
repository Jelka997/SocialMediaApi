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
    { private GroupRepository groupRepository = new GroupRepository();
        private UserRepository userRepository = new UserRepository();
        private UserDbRepository userDbRepository;

        public UserController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if(page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be greater then zero.");
            }
            try
            {  List<User> users = userDbRepository.GetPaged(page, pageSize);
                int totalCount = userDbRepository.CountAll();
                Object result = new
                {
                    Data = users,
                    TotalCount = totalCount
                };
                return Ok(result);
            }
            catch (Exception ex) { return Problem("An error occurred while fetching users."); }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            try
            {
                User user = userDbRepository.GetByIdDb(id);
                if (user == null) { return NotFound(); }
                return Ok(user);
            }
            catch (Exception ex) { return Problem("An error occurred while fetching users."); }

        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.LastName) || string.IsNullOrWhiteSpace(newUser.Birthday.ToString()))
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                return Ok(userDbRepository.CreateNewUser(newUser));
            }
            catch (Exception ex) { return Problem("An error occurred while fetching users."); }
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            if (uUser == null || string.IsNullOrWhiteSpace(uUser.Username) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.LastName) || string.IsNullOrWhiteSpace(uUser.Birthday.ToString()))
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                uUser.Id = id;
                User user = userDbRepository.UpdateUser(id, uUser);
                if(user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex) { return Problem("An error occurred while fetching users."); }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool deleted = userDbRepository.DeleteUser(id);
                if (deleted)
                {
                    return NoContent();
                }
                return NotFound();
            } catch (Exception ex) { return Problem("An error occurred while fetching users.");}
        }
    }
}
