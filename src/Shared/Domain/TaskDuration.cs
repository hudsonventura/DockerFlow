using System;


namespace DockerFlow.Domain;

public class TaskDuration
{
    public string container_id { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public int duration { get; set; } //in seconds
    public decimal percent { get; set; }
    public SystemLog.SystemLogType status { get; set; }
}
