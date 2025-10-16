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
        private readonly GroupMembershipDbRepository  groupMembershipDbRepository;
        private readonly GroupDbRepository  groupDbRepository;
        private readonly UserDbRepository  userDbRepository;

        public UserGroupsController(IConfiguration configuration)
        {
            groupDbRepository = new GroupDbRepository(configuration);
            userDbRepository = new UserDbRepository(configuration);
            groupMembershipDbRepository = new GroupMembershipDbRepository(configuration);
        }

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
    
        [HttpPut]
        public ActionResult<User> Add(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            if (!UserRepository.Data.ContainsKey(userId))
            {
                return NotFound();
            }
            
            User user = UserRepository.Data[userId];
            Group group = GroupRepository.Data[groupId];
            group.Users.Add(user);
            user.Groups.Add(group);
            userRepository.SaveData();
            
            return Ok(user);
        }
        
        [HttpDelete]
        public ActionResult<User> Remove(int groupId, int userId)
        {
            if (!GroupRepository.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            if (!UserRepository.Data.ContainsKey(userId))
            {
                return NotFound();
            }
            
            User user = UserRepository.Data[userId];
            Group group = GroupRepository.Data[groupId];
            group.Users.Remove(user);
            user.Groups.Remove(group);
            userRepository.SaveData();
            
            return NoContent();
        }
        
        [HttpPut("users")]
        public ActionResult<Group> AddUser(int groupId, int userId)
        {
            Group group = groupDbRepository.GetById(groupId);
            if (group == null)
            {
                return NotFound($"Group {groupId} not found");
            }


            User user = userDbRepository.GetByIdDb(userId);
            if (user == null)
            {
                return NotFound($"User {userId} not found");
            }

            if (group.Users.Contains(user))
            {
                return Conflict($"User {userId} already exists");
            }

            try
            {
                if (!groupMembershipDbRepository.AddUser(groupId, userId))
                {
                    return Conflict($"User {userId} already exists in the group {groupId}");
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating the link.");
            }
        }

        [HttpDelete("users")]
        public ActionResult RemoveUser(int groupId, int userId)
        {
            try
            {
                bool isDeleted = groupMembershipDbRepository.RemoveUserFromGroup(groupId, userId); 
                if (isDeleted)
                {
                    return NoContent(); 
                }
                return NotFound($"Group/User not found."); 
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while removing user from the group.");
            }
        }
    }
}
