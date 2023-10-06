using DockerFlow.Services;

namespace DockerFlow.Domain;

public class Container
{
    public string serviceName { get; set; }
    public string containerID { get; set; }
    public string Status { get; set; }

    public void Start(){
        DockerAPI.operateContainer(this, "start");
    }

    public void Stop(){
        DockerAPI.operateContainer(this, "stop");
    }

    public void Restart(){
        DockerAPI.operateContainer(this, "restart");
    }
}
