# dockerlogflow

A system similar to Apache AirFlow interface, but based on Docker. It works by collecting Docker logs, and because that, it works with any programming languages.<br>
<br>
<br> 
### Using
Exemplica como iniciar
<br>

``` bash
docker compose up
```
This will start and collect Docker logs, but you must tell it when each task starts and ends. To do this, you must first choose your language and install the lib:


### C#
``` bash
dotnet add package XXXXXXXXXXXXXXXXXX
```

``` C#
string task_name = $"Apresentar Itens {thread}";
Task task = new Task(task_name);
task.Start();
task.Info("Just an info");
task.Error("An error"); //It will cause a task crash (a red block)
task.Debug("It will only appear in a non-production environment");
task.End();
```