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
            return Ok(userDbRepository.GetByIdDb(id));
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User newUser)
        {
            return Ok(userDbRepository.CreateNewUser(newUser));
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] User uUser)
        {
            return Ok(userDbRepository.UpdateUser(id, uUser));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(userDbRepository.DeleteUser(id));
        }

        /* private int SracunajNoviId(List<int> identifikatori)
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
         }*/
    }
}
