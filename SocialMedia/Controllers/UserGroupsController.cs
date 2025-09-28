using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers
{
    [Route("api/groups/{groupId}/users/{userId}")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private GroupRepository groupRepository = new GroupRepository();
        private UserRepository userRepository = new UserRepository();

        [HttpGet]
        public ActionResult<List<User>> Get(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            if (!UserRepository.Data.ContainsKey(userId))
            {
                return NotFound();
            }

            List<Group> allGroups = GroupRepository.Data.Values.ToList();
            List<User> allUser = UserRepository.Data.Values.ToList();
            User odabraniUser = new User();
            foreach(Group group in allGroups)
            {
                foreach (User user in group.Users) {
                    if (userId == user.Id)
                    {
                        odabraniUser = user;
                    }
                }
            }

            return Ok(odabraniUser);
        }
    }
}
