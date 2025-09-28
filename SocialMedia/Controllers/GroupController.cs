using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController : ControllerBase
{
    private GroupRepository groupRepository = new GroupRepository();
    private UserRepository userRepository = new UserRepository();
    
    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        List<Group> groups = GroupRepository.Data.Values.ToList();
        return Ok(groups);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Group> GetById(int id)
    {
        if (!GroupRepository.Data.ContainsKey(id))
        {
            return NotFound();
        }
        return Ok(GroupRepository.Data[id]);
    }
}
