namespace DockerFlow.Domain;

public class CommonLog : Log
{
    public CommonLog()
    {
    }

    public CommonLog(Log log)
    {
        this.id = log.id;
        this.container_id = log.container_id;
        this.timestamp = log.timestamp;
        this.info = log.info;
    }

    
}
