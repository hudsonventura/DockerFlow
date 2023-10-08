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
    var dbContext = new DataBaseContext(appsettings);

    while(true){
        Thread.Sleep(1000*2);
        
        foreach (var container in containers)
        {
            Console.WriteLine("Foreach");
            try
            {
                Console.WriteLine($"Tentando obter os logs do container {container.serviceName} {container.containerID} ...");
                var atual = logs_all.Where(x => x.containerID == container.containerID).FirstOrDefault();
                //if(atual == null || atual.last_update == DateTime.MinValue){ //verifica se o registro do container já existe na lista de logs para pegar o ultimo horario 
                    
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
                    logs_all.Add(new ContainerLogs(){
                        containerID = container.containerID,
                        logs = logs,
                        last_update = logs.Max(x => x.timestamp)
                    });
                /*}else{
                    DateTime last_check = logs_all.Where(x => x.containerID == container.containerID).LastOrDefault().last_update.AddSeconds(1);
                    var logs = DockerAPI.getContainerLog(container, last_check);

                    //adiciona os novos logs na posiçao já existente
                    logs_all.Where(x => x.containerID == container.containerID).FirstOrDefault().logs.AddRange(logs);
                    logs_all.Where(x => x.containerID == container.containerID).FirstOrDefault().last_update = logs.Max(x => x.timestamp);
                }*/
                Console.WriteLine($"Tentando obter os logs do container {container.serviceName} {container.containerID} ... Sucesso!");
            }
            catch (System.Exception error)
            {
                Console.WriteLine($"Falha ao obter os logs do {container.serviceName} ({container.containerID}): {error.Message}");
            }
        }

        
        
        
         try
        {       //salva os logs no banco de dados
            Console.WriteLine("Tentando salvar os logs ... ");
            var logs = logs_all.SelectMany(x => x.logs).ToList();
            if (logs.Count() > 0)
            {
                Console.WriteLine($"Total de {logs.Count()} itens de log");
                
                var common_logs = logs.Where(x => x.system == false).Select(x => new CommonLog(x)).ToList();
                dbContext.logs.AddRange(common_logs);
                
                var system_logs = logs.Where(x => x.system == true).ToList().Select(log => new SystemLog(log)).ToList();
                dbContext.system_logs.AddRange(system_logs);

                //salva os logs no banco
                dbContext.SaveChanges();
                Console.WriteLine($"Tentando salvar os logs ... Sucesso! Salvo {logs.Count()}");
                foreach (var logs_container in logs_all)
                {
                    logs_container.logs.Clear(); //limpa a lista de logs para nao duplicar nada no banco de dados
                }
            }
            Console.WriteLine("Tentando salvar os logs ... Zero itens.");
            continue;
        }
        catch (System.Exception error)
        {
            Console.WriteLine($"Tentando salvar os logs ... Falha! {error.InnerException.Message}");
        } 

        Console.Clear();
        foreach (var container in containers){
            //Console.WriteLine($"Container {container.serviceName} contem {logs_all[container.containerID].Count()} itens no log");
        }
        Console.WriteLine($"Total de {logs_all.Count()} containeres");
    }
}
        














while(true){
    Thread.Sleep(1000*60);
}