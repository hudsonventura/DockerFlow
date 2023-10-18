# DockerFlow

A system similar to Apache AirFlow interface, but based on Docker. It works by collecting Docker logs, and because that, it works with any programming languages.<br>
<br>
![Interaface](interface_example.png)
<br>
<br> 
## Get stating
The DockerFlow works collecting logs from all Docker containers. For that, just start:
<br>

``` bash
docker compose up -d
```
This will start and collect Docker logs. But to create the interface with tasks like the figure above, you must tell it when each task starts and ends. To do this, you must first choose your language and install the lib:


### C# Console app
``` bash
dotnet add package DockerFlow
```

``` C#
string task_name = $"This is the name of your task";
Task task = new Task(task_name);
task.Start();

//this level is going show a message com DockerFlow web interface console
task.Info("Just an info");
task.Error("An error"); 
task.Debug("It will only appear in a non-production environment");


task.ErrorEnd("A message");     //this level is going a waning on task end
task.WaningEnd("A message");    //this level is going a crash on task
task.SuccessEnd();              //this level is going to finish a task normally
```

### C# Web API
``` bash
dotnet add package DockerFlow
```

<br>

If you just want to log the request and reponse
``` C#
app.UseLogRequestMidleware();
```

But, if you want to log the request and reponse and identify the request like the tasks, for each received request, you can use something like this
``` C#
app.UseLogRequestMidleware(Location.Header, "my_id");
```
<br>

or this
``` C#
app.UseLogRequestMidleware(Location.Query, "another_id");
```
<br>

or this
``` C#
app.UseLogRequestMidleware(Location.Body_json, "my_other_property.xurupita");
```
<br>

In the last case, consider the below json. You can go down as many levels as necessary. 
``` json
{
    "Incident_Number": "IR155669",
    "CaseID": "8000001229",
    "state":"Working",
    "my_other_property": {
        "id": "xurupita"
    }
}
```