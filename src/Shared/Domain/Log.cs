namespace DockerFlow.Domain;

public class Log
{
    
    

    public Guid id { get; set; } = Guid.NewGuid(); //An id to save log in db
    public string container_id { get; set; } //ID of the container (if your are using docker-compose, each execution make a new ID)

    public DateTime timestamp {get; set;} //The datetime of log information
    public string info { get; set; } //The log
    public bool system = false; //If the log is a simple container log or it is a log of Docker-LogFlow


}
