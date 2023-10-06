using Microsoft.AspNetCore.Mvc;
using DockerFlow.Services;
using Shared.Services;


namespace api.Controllers;

[ApiController]
[Route("/Container/Logs")]
public class ContainerLogsController : ControllerBase
{
    private DataBaseContext _dbContext;

    public ContainerLogsController(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }



    /// <summary>
    /// List logs from a container by ID
    /// </summary>
    /// <param name="container_id"></param>
    /// <returns></returns>
    [HttpGet("/Container/Logs/{container_id}")]
    public IActionResult Logs(string container_id){
        var containers = DockerAPI.getContainersCreated();
        if(!containers.Any(x => x.containerID == container_id)){
            return NotFound($"No container found with id {container_id}");
        }
        
        var logs = _dbContext.logs.Where(x => x.container_id == container_id).ToList();
        if(logs.Count() == 0){
            return NoContent();
        }
        return Ok(logs);
    }


    /// <summary>
    /// List system logs (from Docker-LogFlow) from a container by ID
    /// </summary>
    /// <returns>teste</returns>
    [HttpGet("/Container/SystemLogs/{container_id}")]
    public IActionResult SystemLogs(string container_id){
        var containers = DockerAPI.getContainersCreated();
        if(!containers.Any(x => x.containerID == container_id)){
            return NotFound($"No container found with id {container_id}");
        }

        var logs = _dbContext.system_logs.Where(x => x.container_id == container_id).ToList();
        if(logs.Count() == 0){
            return NoContent();
        }
        return Ok(logs);
    }

}
