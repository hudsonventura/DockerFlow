using Task = DockerFlow.Task;


Task task = new Task($"Apresentar Itens t");

task.Start();
task.Info("Your message");      
task.Error("Your message");
task.Debug("Your message");     //it will show just in debug mode
task.Warnning("Your message");

//task.WaningEnd();            //it will end with some warnning, and the task on web interface will be YELLOW
task.ErrorEnd("Your message");  //it will end with some error, and the task on web interface will be RED
//task.SuccessEnd();            //it will end with success, and the task on web interface will be GREEN

try
{
    task.Info("Test"); //it will case an exception, because the task has ended like above.
}
catch (System.Exception)
{
    
}


Task task2 = new Task($"Role creation");

task2.Start();
task2.Info("I't just a info message");      
task2.Error("It's an ERROR message");
task2.Debug("Debug =D"); 
task2.Warnning("Warning, like when the Rockman goes to defeat a boss");

task2.WaningEnd("The object launch some waning, and I'm informing you. Be happy =)");          

    
    

//Test with many threads
for (int i = 0; i < 10; i++)
{
    System.Threading.Tasks.Task.Run(() => run(i));
    Thread.Sleep(200);
}


void run(int thread){
    Task task = new Task($"Apresentar Itens {thread}");
    task.Start();

    task.Info("It's just an info");
    task.Error("It's some error. Please pay attention.");

    for (int i = 0; i < 10; i++)
    {
        if(i % 10 == 0){
            Console.WriteLine("It's just a test and it will not be a system message, bas will be logged");
        }
        Thread.Sleep(1000);
    }
    Random random = new Random();
    int rand = random.Next(1, 100);
    Thread.Sleep(rand*100);

    if(rand > 96){
        task.ErrorEnd($"Random value is {rand}");
        return;
    }
    if(rand > 98){
        task.WaningEnd($"Random value is {rand}");
        return;
    }
    task.SuccessEnd(); //The program '[40998] Example.dll' has exited with code 0 (0x0).
    
}


Thread.Sleep(60*1000);

