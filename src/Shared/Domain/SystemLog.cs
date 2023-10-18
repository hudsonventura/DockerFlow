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

            

            switch (info)
            {
                case string s when s.StartsWith("Starting task"):
                    this.type = SystemLogType.Start;
                    break;
                
                case string s when s.StartsWith("Start of request"):
                    this.type = SystemLogType.Start;
                    break;

                case string s when s.StartsWith("End of task with success"):
                    this.type = SystemLogType.SuccessEnd;
                    break;
                    
                case string s when s.StartsWith("End of request"):
                    this.type = SystemLogType.SuccessEnd;
                    break;

                case string s when s.StartsWith("End of task with some critial error"):
                    this.type = SystemLogType.ErrorEnd;
                    break;

                case string s when s.StartsWith("End of task with some warnning"):
                    this.type = SystemLogType.WarnningEnd;
                    break;
                
                case string s when s.StartsWith("ERROR"):
                    this.type = SystemLogType.Error;
                    break;
            
                case string s when s.StartsWith("INFO"):
                    this.type = SystemLogType.Info;
                    break;
                
                case string s when s.StartsWith("WARNNING"):
                    this.type = SystemLogType.Warning;
                    break;

                case string s when s.StartsWith("DEBUG"):
                    this.type = SystemLogType.Debug;
                    break;
            }

            try
            {
                this.info = info.Split("|-|")[1].Trim();
            }
            catch (System.Exception)
            {
                this.info = this.type.ToString();
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
        Nothing = 0,
        Start = 1,
        ErrorEnd = 2,
        WarnningEnd = 3,
        SuccessEnd = 4,
        Info = 5,
        Warning = 6,
        Error = 7, 
        Debug = 8
    }
    
}


