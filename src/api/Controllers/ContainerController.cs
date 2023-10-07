using Microsoft.AspNetCore.Mvc;
using DockerFlow.Services;
using Shared.Services;


namespace api.Controllers;

[ApiController]
[Route("/Container")]
public class ContainerController : ControllerBase
{
    private DataBaseContext _dbContext;

    public ContainerController(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    /// Get the list os created containers (if running or not)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var containers = DockerAPI.getContainersCreated();
            return Ok(containers);
        }
        catch (System.Exception)
        {
            return BadRequest("It's not possible to obtain the containers list");
        }
    }
    

    /// <summary>
    /// Inspect a container by ID
    /// </summary>
    /// <param name="container_id"></param>
    /// <returns></returns>
    [HttpGet("/Container/{container_id}")]
    public IActionResult Get(string container_id)
    {
        try
        {
            var container = DockerAPI.GetContainer(container_id);
            var propeties = DockerAPI.inspectContainer(container);
            return Ok(propeties);
        }
        catch (System.Exception)
        {
            return BadRequest("It's not possible to obtain the containers list");
        }
    }


   
}
