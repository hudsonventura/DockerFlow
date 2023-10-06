using Newtonsoft.Json;

namespace DockerFlow.Domain;


public class Task : IDisposable
{
    public string task_name {get; private set;} = String.Empty;
    private Guid task_id;
    public bool IsStarted {get; private set;} = false;
    

    /// <summary>
    /// Start your task. You must name it.
    /// </summary>
    /// <param name="task_name">A string with the name of task</param>
    public Task(string task_name)
    {
        this.task_name = task_name;
        task_id = Guid.NewGuid();
    }


    
    public void Start(){
        if(tasks.Contains("task_name")){
            throw new Exception("This tasks has already started");
        }
        Add(task_name);
        IsStarted = true;
        System.Console.WriteLine($"{ConsoleCode()} Starting task '{task_name}'");
    }

    public void Info(string text){
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"{ConsoleCode()} INFO '{task_name}' - {text}");
        System.Console.ResetColor();
    }

    public void Debug(string text){
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if(env == "Production"){
            return;
        }
        System.Console.WriteLine($"{ConsoleCode()} DEBUG '{task_name}' - {text}");
    }

    public void Warnning(string text){
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"{ConsoleCode()} WARNNING '{task_name}' - {text}");
        System.Console.ResetColor();
    }
    public void Error(string text){
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"{ConsoleCode()} ERROR '{task_name}' - {text}");
        System.Console.ResetColor();
    }


    public void End(){
        if(!tasks.Contains("task_name")){
            throw new Exception("This tasks has already stoped");
        }
        Leave(task_name);
        IsStarted = false;
        System.Console.WriteLine($"{ConsoleCode()} End of task '{task_name}'");
        this.Dispose();
    }




    public void Dispose()
    {
        GC.SuppressFinalize(this); // Evita que o coletor de lixo chame Dispose() novamente
    }

    private string ConsoleCode(){
        return $" XXX==--> {execution_id} - {task_id} - {task_name} <--==XXX -";
    }






    private static Guid execution_id = Guid.NewGuid();
    private static List<string> tasks = new List<string>();
    private void Add(string task_name){
        if(tasks.Contains(task_name)){
            throw new Exception($"The task '{task_name}' already been added. Choose another name.");
        }
        tasks.Add(task_name);
    }

    private void Leave(string task_name){
        tasks.Remove(task_name);
    }

    
}