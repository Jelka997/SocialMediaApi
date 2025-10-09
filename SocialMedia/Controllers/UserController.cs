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
        private UserDbRepository userDbRepository = new UserDbRepository();

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return Ok(userDbRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            User user = userDbRepository.GetByIdDb(id);
            if(user == null) { return NotFound();}
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.LastName) || string.IsNullOrWhiteSpace(newUser.Birthday.ToString()))
            {
                return BadRequest();
            }
            return Ok(userDbRepository.CreateNewUser(newUser));
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            var user = userDbRepository.GetByIdDb(id);
            if (string.IsNullOrWhiteSpace(uUser.Username) || string.IsNullOrWhiteSpace(uUser.Name) || string.IsNullOrWhiteSpace(uUser.LastName) || string.IsNullOrWhiteSpace(uUser.Birthday.ToString()))
            {
                return BadRequest();
            }
            if (user == null) { return NotFound(); }
            
            return Ok(userDbRepository.UpdateUser(id, uUser));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = userDbRepository.GetByIdDb(id);
            if (user == null) { return NotFound(); }
            userDbRepository.DeleteUser(id);
            return NoContent();
        }
    }
}
