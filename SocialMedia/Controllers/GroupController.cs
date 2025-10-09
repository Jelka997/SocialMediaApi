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

    private readonly GroupDbRepository  groupDbRepository;

    public GroupController(IConfiguration configuration)
    {
        groupDbRepository = new GroupDbRepository(configuration);
    }
    
    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        return Ok(groupDbRepository.GetAll());
    }
    
    [HttpGet("{id}")]
    public ActionResult<Group> GetById(int id)
    {
        try
        {
            Group group = groupDbRepository.GetById(id);
            if (group  == null)
            {
                return NotFound($"Group with ID {id} not found.");
            }
            return Ok(group);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while fetching the group.");
        }
    }
    
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        if (newGroup == null || string.IsNullOrWhiteSpace(newGroup.Name)  || string.IsNullOrWhiteSpace(newGroup.DateCreated.ToString()))
        {
            return BadRequest("Invalid group data.");
        }

        try
        {
            Group createdGroup = groupDbRepository.Create(newGroup); 
            return Ok(createdGroup);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while creating the group.");
        }
    }
    
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            bool isDeleted = groupDbRepository.Delete(id); 
            if (isDeleted)
            {
                return NoContent(); 
            }
            return NotFound($"Group with ID {id} not found."); 
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while deleting the group.");
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Group> Update(int id, [FromBody] Group group)
    {
        if (group == null || string.IsNullOrWhiteSpace(group.Name) || string.IsNullOrWhiteSpace(group.DateCreated.ToString()))
        {
            return BadRequest("Invalid group data.");
        }

        try
        {
            group.Id = id;
            Group updatedGroup = groupDbRepository.Update(group); 
            if(updatedGroup == null)
            {
                return NotFound();
            }
            return Ok(updatedGroup); 
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while updating the group.");
        }
    }
}
