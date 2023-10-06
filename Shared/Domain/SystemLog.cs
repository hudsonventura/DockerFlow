using System.Text;

namespace DockerFlow.Domain;

public class SystemLog : Log
{
    

    public SystemLog()
    {
    }

    public SystemLog(Log log)
    {
        
        try
        {
            if(log.container_id == "c57f2d532638eda350c8944e38bfe281c128b4bdace34ba3daeb93f3af466cf5"){
                Console.WriteLine();
            }
            this.id = log.id;
            this.container_id = log.container_id;
            this.timestamp = log.timestamp;
            this.system = true;
    
            var components = log.info.Split(" <--==XXX - ");
            var system_components = components[0].Substring(10).Split(" - ");
            
            //this.execution_id = Guid.Parse(system_components[0].Split("  XXX==--> ")[1]);
            this.execution_id = Guid.Parse(system_components[0]);
            this.task_id = Guid.Parse(system_components[1].ToString());
            this.task_name = system_components[2].ToString();
            this.system = true;
            
            var info = components[1].ToString();
            Console.WriteLine(task_name);
            if(info == null){
                Console.WriteLine();
            }
            this.info = info;
            if(info.StartsWith("Starting task")){
                this.type = SystemLogType.Start;
            }
    
            if(info.StartsWith("End of task")){
                this.type = SystemLogType.End;
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }




   
    public Guid execution_id { get; set; } //thread_id
    public Guid task_id { get; set; } //task id
    public string task_name { get; set; } //Task user identification
    public SystemLogType type { get; set; }

    public enum SystemLogType{
        Start = 0,
        End = 1,
        Info = 2,
        Warning = 3,
        Error = 4, 
        PartialError = 5
    }
    
}


