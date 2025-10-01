using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers;

[Route("api/groups/{groupId}/users")]
[ApiController]
public class GroupUsersController : ControllerBase
{
    private GroupRepository groupRepository = new GroupRepository();
    private UserRepository userRepository = new UserRepository();

    [HttpGet]
    public ActionResult<List<User>> Get(int groupId)
    {
        if (!GroupRepository.Data.ContainsKey(groupId))
        {
            return NotFound();
        }

        List<Group> allGroups = GroupRepository.Data.Values.ToList();
        List<User> odabraniUseri = new List<User>();

        foreach (Group group in allGroups)
        {
            if (group.Id == groupId)
            {
                foreach (User user in group.Users)
                {
                    odabraniUseri.Add(user);
                }
            }

        }
        
        return Ok(odabraniUseri);
    }
}