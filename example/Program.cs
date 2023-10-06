using Task = DockerFlow.Domain.Task;


for (int i = 0; i < 10; i++)
{
    System.Threading.Tasks.Task.Run(() => run(i));
    Thread.Sleep(200);
}


Thread.Sleep(5000000);






void run(int thread){
    Task task = new Task($"Apresentar Itens {thread}");
    task.Start();
    task.Info("Teste de info");
    task.Error("Teste de erro");
    for (int i = 0; i < 10; i++)
    {
        if(i % 10 == 0){
            Console.WriteLine("Apenas um teste que não deve ser de sistema");
            //logging.WriteLine($"Hello, World! N.{thread} V.{i}");
            //logging.Dump(new Teste());
        }
        //logging.WriteLine($"Hello, World! N.{thread} V.{i}");
        Thread.Sleep(1000);
    }
    task.End();
}


