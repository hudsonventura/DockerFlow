using Microsoft.AspNetCore.Mvc;
using DockerFlow.Services;
using Shared.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using DockerFlow.Domain;
using Task = Shared.Domain.Task;


namespace api.Controllers;

[ApiController]
[Route("/Container/Tasks")]
public class ContainerTasksController : ControllerBase
{
    private DataBaseContext _dbContext;

    public ContainerTasksController(DataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }




    /// <summary>
    /// DOCUMENTAR
    /// </summary>
    /// <param name="container_id"></param>
    /// <returns></returns>
    [HttpGet("/Container/Tasks/{container_id}")]
    public IActionResult TasksDistinctOrder(string container_id){
        var containers = DockerAPI.getContainersCreated();
        if(!containers.Any(x => x.containerID == container_id)){
            return NotFound($"No container found with id {container_id}");
        }

        var logs = _dbContext.system_logs
                    .Where(x => x.container_id == container_id)
                    .OrderBy(x => x.timestamp)
                    .ToList();

        if(logs.Count() == 0){
            return NoContent();
        }

        var tasks_names = logs.Select(x => x.task_name).ToList().Distinct();

        Dictionary<string, List<Task>> ret = new Dictionary<string, List<Task>>();
        foreach (var task_name in tasks_names)
        {
            List<Task> tasks = new List<Task>();
            var task_logs = logs.Where(x => x.task_name == task_name).OrderBy(x => x.timestamp).ToList();
            var executions_grouped = task_logs.GroupBy(x => x.execution_id);
            
            foreach (var executions in executions_grouped)
            {
                Task task = new Task();
                foreach (var log in executions) //verifica cada possivel status de uma task nos logs
                {
                    if(log.type == SystemLog.SystemLogType.Start){
                        task = new Task(){
                            task_name = log.task_name,
                            start = log.timestamp,
                            status = Task.Status.Initialized
                        };
                    }Console.WriteLine($"Tentando salvar os logs ... Sucesso! Salvo {logs.Count()}");

                    if(log.type == SystemLog.SystemLogType.End){
                        task.end = log.timestamp;
                        task.status = Task.Status.Success;
                    }
                    
                }
                tasks.Add(task);
            }
            ret.Add(task_name, tasks);
        }

        return Ok(ret);
    }

}
