using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using DockerFlow.Domain;
using Newtonsoft.Json.Linq;
using Console = System.Console;
using Exception = System.Exception;

namespace DockerFlow.Services;

public class DockerAPI
{
    /*
        Obter o log de um container
        sudo curl -X GET --unix-socket /var/run/docker.sock "http://localhost/containers/0e097787d6356b112eb536c6db17580a743983d1404c713e68ad7b6e77e238d8/logs?stdout=1&stderr=1&timestamps=1&since=1629964800" --output -

        Obter a lista de containers
        sudo curl -XGET --unix-socket /var/run/docker.sock -H 'Content-Type: application/json' http://localhost/containers/json?all=1 --output -
    */

    /// <summary>
    /// Lista todos os containers criados
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static List<Container> getContainersCreated()
    {
        string response = callAPI("GET", "http://localhost/containers/json?all=1");
        //string response = File.ReadAllText("./examples/containers.json?all.json");

        //Console.WriteLine(response);
        
        if (response == "") {
            throw new Exception("I ran curl but the response was empty");
        }

        var objs = JArray.Parse(response);

        List<Container> result = new List<Container>();
        foreach (var obj in objs)
        {
            string id  = obj["Id"].ToString();
            var name = obj["Names"][0].ToString().Substring(1);
            var status = obj["Status"].ToString();

            if(name.StartsWith("docker-flowlog-")){
                continue;
            }

            Container add = new Container(){
                containerID = id,
                serviceName = name,
                Status = status
            };
            result.Add(add);
        }

        return result;
        
    }

    public static Container GetContainer(string container_id){
        var containers = getContainersCreated();
        foreach (var container in containers)
        {
            if(container.containerID == container_id){
                return container;
            }
        }
        throw new Exception($"Container {container_id} not found");
    }

    public static List<Log> getContainerLog(Container container, DateTime last_check){
        long last_check_timestamp = ((DateTimeOffset)last_check).ToUnixTimeSeconds();
        string response = callAPI("GET", $"http://localhost/containers/{container.containerID}/logs?stdout=1&stderr=1&timestamps=1&since={last_check_timestamp}");
        //string response = File.ReadAllText("./examples/container.log");
        
        //Console.WriteLine(response);

        List<Log> result = new List<Log>();

        string line;
        StringReader reader = new StringReader(response);
        int contador = 0;
        bool IsAspnet_request = false;
        string aspnet_request = "";
        while ((line = reader.ReadLine()) != null){
            if(line == String.Empty){
                continue; //ignora linhas vazias
            }
            try
            {
                //pode ser que info nao seja UTF-8
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(line);
                line = Encoding.UTF8.GetString(utf8Bytes);

                DateTime timestamp = matchDatetime(line);
                string log = line.Substring(39);

                
                bool system = (log.Contains("XXX==-->") && log.Contains("<--==XXX"))? true : false;
                if(system){
                    if(log.Contains("Start of request")){
                        IsAspnet_request = true;
                    }
                    if(log.Contains("End of request")){
                        IsAspnet_request = false;
                        result.Add(new Log(){
                            container_id = container.containerID,
                            timestamp = timestamp.ToUniversalTime(),
                            info = aspnet_request,
                            system = true
                        });  
                        continue; 
                    }
                }
                if(IsAspnet_request == true && !log.Contains("Start of request")){
                    aspnet_request += log+Environment.NewLine;
                    continue;
                }

                result.Add(new Log(){
                    container_id = container.containerID,
                    timestamp = timestamp.ToUniversalTime(),
                    info = log,
                    system = system
                });   

                contador++;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Não foi possivel converter a linha em log: '{line}'\r\n. Error: {error.Message}");
            }
        }
        return result;
    }

    private static DateTime matchDatetime(string input)
    {
        string pattern = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{9}Z";
        Match match = Regex.Match(input, pattern);
        
        if (match.Success)
        {
            // Extraia a parte correspondente à data e hora
            string dateTimeString = match.Value;
            
            // Tente converter a string em um objeto DateTime
            if (DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                Console.WriteLine("Não foi possível converter a data e hora.");
            }
        }
        return DateTime.MinValue;
    }

    public static void operateContainer(Container container, string command)
    {
        if (container.containerID == null) {
            string msg = $"Service {container.serviceName}: I cannot found the container ID";
            Console.WriteLine(msg); //, Console.typeMessage.FAIL);
            throw new Exception(msg);
        }

        //realiza o restart do container
        string retorno = callAPI("POST", $"http://localhost/containers/{container.containerID}/{command}");
        retorno = retorno.Replace("{\"message\":\"page not found\"}", "");
        if (retorno.Contains("No such container"))
        {
            string msg = $"Service {container.serviceName}: No such container. Service {container.serviceName}, ID: {container.containerID}";
            Console.WriteLine(msg); //, Console.typeMessage.FAIL);
            throw new Exception(msg);
        }

        if (retorno == "")
        {
            string msg = $"Service {container.serviceName}: The docker.sock did not repond some valid";
            Console.WriteLine(msg); //, Console.typeMessage.FAIL);
            throw new Exception(msg);
        }
        Console.WriteLine($"Service {container.serviceName}: Container restarted successfully"); //, Console.typeMessage.SUCESS);
    }


    public static dynamic inspectContainer(Container container){
        return callAPI("GET", $"http://localhost/containers/{container.containerID}/json");
    }

    private static string callAPI(string type, string args)
    {
        Process process = new Process();
        process.StartInfo.FileName = "curl";
        process.StartInfo.Arguments = $"--silent -X{type} --unix-socket /var/run/docker.sock -H 'Content-Type: application/json' {args} --output -";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string err = process.StandardError.ReadToEnd();
        process.WaitForExit();

        //System.Console.WriteLine(output);

        if (output == "" && err == "")
        {
            throw new Exception(process.BasePriority.ToString());
        }
        if (err != "")
        {
            throw new Exception(err);
        }


        if (output == null)
        {
            throw new Exception(err);
        }

        return output.Replace("{\"message\":\"page not found\"}", "");
    }
}
