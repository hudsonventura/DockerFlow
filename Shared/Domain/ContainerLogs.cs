namespace DockerFlow.Domain;

public class ContainerLogs
{
    public DateTime last_update {get; set;} = DateTime.MinValue;
    public string containerID { get; set; }
    public List<Log> logs { get; set; }
}
