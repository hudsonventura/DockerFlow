namespace Shared.Domain;

public class Task
{
    public Guid task_id { get; set; } 
    public string task_name { get; set; } 
    public Status status { get; set; } = Status.NoInitialized;
    public DateTime start  { get; set; } 
    public DateTime end  { get; set; } 

    public string message { get; set; }

    public enum Status{
        NoInitialized = 0,
        Initialized = 1,
        Error = 2,
        Warnning = 3,
        Success = 4, 
        PartialError = 5
    }
}
