using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DockerFlow.Services;
using Shared.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using DockerFlow.Domain;
using Task = Shared.Domain.Task;
using Shared.Domain;

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
                            status = Task.Status.Initialized,
                            message = log.info,
                            task_id = log.task_id
                        };
                    }

                    if(log.type == SystemLog.SystemLogType.ErrorEnd || log.type == SystemLog.SystemLogType.WarnningEnd || log.type == SystemLog.SystemLogType.SuccessEnd){
                        task.end = log.timestamp;
                        task.status = (Task.Status)Convert.ToInt32(log.type);
                        task.message = log.info;
                    }
                    
                }
                tasks.Add(task);
            }
            ret.Add(task_name, tasks);
        }

        return Ok(ret);
    }


    [HttpGet("/Container/Tasks/{container_id}/{task_id}")]
    public IActionResult TaskLog(string container_id, Guid task_id){
        var containers = DockerAPI.getContainersCreated();
        if(!containers.Any(x => x.containerID == container_id)){
            return NotFound($"No container found with id {container_id}");
        }

        var logs = _dbContext.system_logs
                    .Where(x => x.container_id == container_id && x.task_id == task_id)
                    .OrderBy(x => x.timestamp)
                    .ToList();
        if(logs.Count() == 0){
            return NoContent();
        }

        return Ok(logs);
    }


    [HttpGet("/Container/Tasks/{container_id}/durations")]
    public IActionResult TasksDuration(string container_id){
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

        var executions = logs.GroupBy(x => x.execution_id);


        Dictionary<Guid, TaskDuration> ret = new Dictionary<Guid, TaskDuration>();
        foreach (var execution in executions){
            var firstExecution = execution.First(); // Primeiro elemento do grupo
            var lastExecution = execution.Last();   // Último elemento do grupo
            ret.Add(execution.Key, new TaskDuration(){
                container_id = container_id,
                start = firstExecution.timestamp,
                end = lastExecution.timestamp,
                duration = (int)(lastExecution.timestamp - firstExecution.timestamp).TotalMilliseconds
            });
        }


        //obtain the max total time
        var max = ret.Max(x => x.Value.duration);
        foreach (var item in ret)
        {
            decimal percent = (decimal)item.Value.duration/max;
            item.Value.percent = percent;
        }

        return Ok(ret);
    }
}
