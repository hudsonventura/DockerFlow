using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;


namespace DockerFlow.Midlewares;
public class LogRequest
{
    private readonly RequestDelegate _next;
    private readonly Location _location;
    private readonly string _location_id;

    private string _headers;
    private string _endpoint;
    private string _method;
    private string _body;

    public LogRequest(RequestDelegate next)
    {
        _next = next;
    }
    public LogRequest(RequestDelegate next, Location location, string id)
    {
        _next = next;
        _location = location;
        _location_id = id;
    }

    public Type _type { get; set; }
    public LogRequest(RequestDelegate next, Location location, Type type, string id)
    {
        _next = next;
        _location = location;
        _location_id = id;
        _type = type;
    }


    public async Task Invoke(HttpContext context)
    {
        //Gera um ID para a request
        var Request_id = Guid.NewGuid().ToString();
        context.Items["Request_id"] = Request_id;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Extrai as informações do contexto
        _headers = string.Join(Environment.NewLine, context.Request.Headers.Select(header => $"{header.Key}: {header.Value}"));
        var host = context.Request.Host;
        _endpoint = context.Request.Path;
        _method = context.Request.Method;
        var remoteIpAddress = context.Connection.RemoteIpAddress;
        _body = await ReadRequestBody(context.Request);



        

        var named_id = getIDValue(_location, _location_id);

        string log_request = $"REQUEST: {Request_id}{Environment.NewLine}IP: {remoteIpAddress}{Environment.NewLine}ID: {named_id}{Environment.NewLine}{Environment.NewLine}{_method} {host}{_endpoint}{Environment.NewLine}{_headers}{Environment.NewLine}{Environment.NewLine}{_body}";

        Stream originalbody = context.Response.Body;

        System.Exception request_exception = null;

        string response_body = "";
        try
        {
            using (var memstream = new MemoryStream())
            {
                context.Response.Body = memstream;

                // Chama o próximo middleware na cadeia
                await _next(context);

                memstream.Position = 0;
                response_body = new StreamReader(memstream).ReadToEnd();

                memstream.Position = 0;
                await memstream.CopyToAsync(originalbody);
            }
        }
        catch(Exception error){ //devolve o erro em caso de algum exception
            request_exception = error;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";
            string minhaString = "Minha string de exemplo";
            byte[] bytes = Encoding.UTF8.GetBytes(error.ToString());
            ReadOnlyMemory<byte> memory = new ReadOnlyMemory<byte>(bytes);
            context.Response.Body = originalbody;
            await context.Response.Body.WriteAsync(memory);
        }
        finally
        {
            context.Response.Body = originalbody;
        }

        

        var response = context.Response;
        var response_statusCode = response.StatusCode;
        var response_headers = string.Join(Environment.NewLine, response.Headers.Select(header => $"{header.Key}: {header.Value}"));

        stopwatch.Stop();
        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        //return $" XXX==--> {execution_id} - {task_id} - {task_name} <--==XXX -";
        string separadorStart = $"{Environment.NewLine} XXX==--> {Request_id} - {named_id} - {_endpoint} <--==XXX - Start of request '{named_id}'{Environment.NewLine}";
        string separadorEnd = $"{Environment.NewLine} XXX==--> {Request_id} - {named_id} - {_endpoint} <--==XXX - End of request '{named_id}'{Environment.NewLine}";
        
        string log_respose;
        if(request_exception == null){
            //return exception captured or OK
            log_respose = $"RESPONSE: {Request_id}{Environment.NewLine}IP: {remoteIpAddress}{Environment.NewLine}StatusCode: {response_statusCode}{Environment.NewLine}Timelapsed: {elapsedMilliseconds}{Environment.NewLine}Exception: false{Environment.NewLine}{Environment.NewLine}{response_headers}{Environment.NewLine}{Environment.NewLine}{response_body}";
        }else{
            //return exception non captured
            log_respose = $"RESPONSE: {Request_id}{Environment.NewLine}IP: {remoteIpAddress}{Environment.NewLine}StatusCode: {response_statusCode}{Environment.NewLine}Timelapsed: {elapsedMilliseconds}{Environment.NewLine}Exception: true{Environment.NewLine}{Environment.NewLine}{response_headers}aaaa{request_exception}";
        }

        Console.WriteLine($"{separadorStart}{log_request}{Environment.NewLine}----------{Environment.NewLine}{log_respose}{Environment.NewLine}{separadorEnd}");

        
    }




    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        // Configure o corpo da requisição para permitir a leitura posterior
        request.EnableBuffering();

        // Lê o corpo da requisição como uma string sem consumi-lo
        using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
        {
            string requestBody = await reader.ReadToEndAsync();

            // Volta ao início do fluxo para que o corpo possa ser lido novamente posteriormente
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBody;
        }
    }


    private string? getIDValue(Location location, string? id){
        if(location == Location.Not || id == null || id == String.Empty){
            return null;
        }

        try
        {
            switch (location)
            {
                case Location.Header: 
                    return _headers.Where(x => x.Equals(id)).FirstOrDefault().ToString();
                
                case Location.Query:
                    return "";
                
                case Location.URLSegment:
                    return "";
                
                case Location.Body_json:
                    JObject jsonObject = JObject.Parse(_body);
                    return getSubJson(jsonObject, _location_id);
                
            }
        }
        catch (System.Exception)
        {
            return null;
        }
        return null;
    }

    private string getSubJson(JObject json, string path){
    {
        // Divide o caminho em partes usando o caractere '.'
        string[] parts = path.Split('.');

        JObject currentObject = json;

        // Percorre as partes do caminho
        foreach (string part in parts)
        {
            if (currentObject.TryGetValue(part, out JToken token))
            {
                if (token is JObject)
                {
                    currentObject = (JObject)token;
                }
                else
                {
                    return token.ToString();
                }
            }
            else
            {
                return null; // Chave não encontrada
            }
        }

        return null; // Caminho inválido
    }
    }
    
}


public static class RequestBodyMiddlewareExtensions
{
    public static IApplicationBuilder UseLogRequestMidleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogRequest>();
    }

    public static IApplicationBuilder UseLogRequestMidleware(this IApplicationBuilder builder, Location location, string id)
    {
        return builder.UseMiddleware<LogRequest>(location, id);
    }

}




    public enum Location{
        Not,
        Header,
        Query,
        URLSegment,
        Body_json
    }

