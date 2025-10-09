using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SocialMedia.Domain;
using SocialMedia.Repositories;

namespace SocialMedia.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController : ControllerBase
{
    private GroupRepository groupRepository = new GroupRepository();
    private UserRepository userRepository = new UserRepository();
    
    private GroupDbRepository groupDbRepository = new GroupDbRepository();
    
    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        return Ok(groupDbRepository.GetAll());
    }
    
    [HttpGet("{id}")]
    public ActionResult<Group> GetById(int id)
    {
        Group group = groupDbRepository.GetById(id);
        if (group  == null)
        {
            return NotFound();
        }
        return Ok(group);
    }
    
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        if (string.IsNullOrWhiteSpace(newGroup.Name)  || string.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
        {
            return BadRequest();
        }

        return Ok(groupDbRepository.Create(newGroup));
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Group group = groupDbRepository.GetById(id);
        if (group == null)
        {
            return NotFound();
        }

        if (group.Id != id) 
        {
            return BadRequest("Id ne odgovara traženoj grupi.");
        }
        
        groupDbRepository.Delete(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Group group)
    {
        if (string.IsNullOrWhiteSpace(group.Name)  || string.IsNullOrWhiteSpace(group.DateCreated.ToString()) || group.Id != id)
        {
            return BadRequest();
        }
        
        groupDbRepository.Update(group);
        return Ok(group);
    }
}
