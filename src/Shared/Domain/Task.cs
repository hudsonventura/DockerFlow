namespace Shared.Domain;

public class Task
{
    public string task_name {get; set; } 
    public Status status {get; set; } = Status.NoInitialized;
    public DateTime start  {get; set; } 
    public DateTime end  {get; set; } 

    public enum Status{
        NoInitialized = 0,
        Initialized = 1,
        Success = 2,
        Warnning = 3,
        Info = 4, 
        FatalError = 5,
        PartialError = 6
    }
}
