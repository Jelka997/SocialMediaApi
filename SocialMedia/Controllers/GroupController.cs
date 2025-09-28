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
    
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        if (string.IsNullOrWhiteSpace(newGroup.Name)  || string.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
        {
            return BadRequest();
        }

        newGroup.Id = SracunajNoviId(GroupRepository.Data.Keys.ToList());
        GroupRepository.Data[newGroup.Id] = newGroup;
        groupRepository.SaveData();

        return Ok(newGroup);
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (!GroupRepository.Data.ContainsKey(id))
        {
            return NotFound();
        }

        GroupRepository.Data.Remove(id);
        groupRepository.SaveData();

        return NoContent();
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
