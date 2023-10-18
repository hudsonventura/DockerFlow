using System.Linq;
using System.Collections.Generic;
using DockerFlow.Domain;
using DockerFlow.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Services;
using Task = System.Threading.Tasks.Task;


var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .Build();
        Console.WriteLine(Directory.GetCurrentDirectory());



            

List<Container> containers = new List<Container>();

//Lista os containeres a cada X tempo
Task task1 = Task.Run(() => getContainersCreated());
void getContainersCreated(){
    while (true)
    {
        try
        {
            Console.WriteLine("Tentando obter a lista de containers ...");
            containers = DockerAPI.getContainersCreated();
            Console.WriteLine($"Tentando obter a lista de containers ... Sucesso! {containers.Count()} ativos.");
            Thread.Sleep(1000*60);
        }
        catch (System.Exception error)
        {
            Console.WriteLine($"Falha ao obter a lista de containers: {error.Message}");
            Thread.Sleep(1000*10);
        }
    }
}






//lista que armazena todos os logs de todos os conaineres até o momento de salvar no banco
List<ContainerLogs> logs_all = new List<ContainerLogs>();

Thread.Sleep(1000*1);



Task task2 = Task.Run(() => getContainerLog());
//coleta os logs e salva no banco de dados
void getContainerLog(){
    

    while(true){
        Thread.Sleep(1000*2);
        
        foreach (var container in containers)
        {
            try
            {
                var dbContext = new DataBaseContext(appsettings);
                Console.WriteLine($"Tentando obter os logs do container {container.serviceName} {container.containerID} ...");
                var atual = logs_all.Where(x => x.containerID == container.containerID).FirstOrDefault();

                //se entrou aqui, o container ainda não teve logs coletados no docker, mas...
                var logs = DockerAPI.getContainerLog(container, DateTime.MinValue.ToUniversalTime());

                //é necessário verificar se o container já possui registro coletado já armazenado no banco
                try
                {
                    DateTime max_date = max_date = dbContext.logs.Where(x => x.container_id == container.containerID).Max(x => x.timestamp);
                    DateTime max_date_system = DateTime.MinValue;
                    try
                    {
                        max_date_system = dbContext.system_logs.Where(x => x.container_id == container.containerID).Max(x => x.timestamp);
                    }
                    catch (System.Exception)
                    {
                        
                    }
                    max_date = max_date > max_date_system ? max_date : max_date_system; //get the major log date
                    logs = logs.Where(x => x.timestamp >= max_date).ToList();
                }
                catch (System.Exception)
                {
                    
                }
                

                //adiciona os logs na lista de ContainerLogs (logs_all)
                var logs_container = new ContainerLogs(){
                    containerID = container.containerID,
                    logs = logs,
                    last_update = logs.Max(x => x.timestamp)
                };

                SalvarLogs(logs_container.logs);

                GC.Collect();

                Console.WriteLine($"Tentando obter os logs do container {container.serviceName} {container.containerID} ... Sucesso!");
            }
            catch (System.Exception error)
            {
                Console.WriteLine($"Falha ao obter os logs do {container.serviceName} ({container.containerID}): {error.Message}");
            }
        }


    }
}
        


void SalvarLogs(List<Log> logs){
    var dbContext = new DataBaseContext(appsettings);
    Console.WriteLine("Tentando salvar os logs ... ");
    if(logs.Count() > 0){
        Console.WriteLine("Tentando salvar os logs ... Zero itens.");
    }
    
    Console.WriteLine($"Tentando salvar os logs ... Total de {logs.Count()} itens de log");
    
    var common_logs = logs.Where(x => x.system == false).Select(x => new CommonLog(x)).ToList();
    dbContext.logs.AddRange(common_logs);
    
    var system_logs = logs.Where(x => x.system == true).ToList().Select(log => new SystemLog(log)).ToList();
    dbContext.system_logs.AddRange(system_logs);

    //salva os logs no banco
    try
    {
        dbContext.SaveChanges();
    }
    catch (System.Exception error)
    {
        Console.WriteLine($"Tentando salvar os logs ... Total de {logs.Count()} itens de log ... Erro: {error.Message}");
    }

    Console.WriteLine($"Tentando salvar os logs ... Total de {logs.Count()} itens de log ... OK!");
    
}











while(true){
    Thread.Sleep(1000*60);
}