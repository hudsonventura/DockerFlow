import react, { useEffect, useState }from "react";
import Log from "../services/Log";
import { useParams } from "react-router-dom";
import Api from '../services/Api';



export default function Container() {
    const { container_id } = useParams();
    
    const console = {
        backgroundColor: "black",
        color: "white",
        fontFamily: "Consolas",
        margin: "10px",
        padding: "20px",
        overflowX: "hidden",
        overflowY: "auto",
        textAlign: "left",
        height: "550px"
    };

    const vertical_progress = {
        position: "relative",
        width: "40px",
        min_height: "240px",
        float: "left",
        margin: "20px",
      };
      


    const[tasks, setTasks] = useState([]);
    useEffect(() => {
        Api.get("/Container/Tasks/"+container_id).then(function(response) 
        {
            setTasks(response.data);
        })
        .catch((error) => {
                throw new Error("Erro ao chamar a API: "+error);
            }
        );
    }, []);


    
    const [consoleContent, setConsoleContent] = useState([]);
    useEffect(() => {
        Api.get("/Container/Logs/"+container_id).then(function(response) 
        {
            setConsoleContent(response.data);
        })
        .catch((error) => {
                throw new Error("Erro ao chamar a API: "+error);
            }
        );
    }, []);


    function handleTaskClick(task_id){
        Api.get("/Container/Tasks/"+container_id+"/"+task_id).then(function(response) 
        {
            setConsoleContent(response.data);
        })
        .catch((error) => {
                throw new Error("Erro ao chamar a API: "+error);
            }
        );
    }

    const [durations, setDurations] = useState([]);
    useEffect(() => {
        Api.get("/Container/Tasks/"+container_id+"/Durations").then(function(response) 
        {
            setDurations(response.data);
        })
        .catch((error) => {
                throw new Error("Erro ao chamar a API: "+error);
            }
        );
    }, []);

    return (

        <>
            <table className="table table-bordered table-sm">
                <tbody>
                    <tr>
                        <td className="running-table" style={{ verticalAlign: "middle" }}>Tempo total</td>
                        {
                            Object.keys(durations).map((key, key1) => {
                                const duration = durations[key];
                                let btnColor = "btn-success"; // Classe padrão
                                let btnClass = "btn-success"; // Classe padrão
                                let animated = "";
                                let text = "";

                                // Use um switch para determinar a classe com base no valor de duration.status
                                switch (duration.status) {
                                    case 1: //Started
                                    btnColor = "#0dcaf0";
                                    btnClass = "btn-info";
                                    animated = "progress-bar-animated";
                                    text = "Running";
                                    break;

                                    case 2: //ErrorEnd
                                    btnColor = "#dc3545";
                                    btnClass = "btn-danger"; 
                                    text = "Ended with some errors";
                                    break;

                                    case 3: //WarnningEnd
                                    btnColor = "#198754";
                                    btnClass = "btn-warning";
                                    text = "Ended with some warning";
                                    break;
                                
                                    case 4: //SuccessEnd
                                    btnColor = "#198754"; 
                                    btnClass = "btn-success";
                                    text = "Ended";
                                    break;

                                    case 5: //Info
                                    btnColor = "#198754"; 
                                    btnClass = "btn-info";
                                    animated = "progress-bar-animated";
                                    text = "Running and some info was informed";
                                    break;


                                    case 6: //Warning
                                    btnColor = "#198754"; 
                                    btnClass = "btn-warning";
                                    animated = "progress-bar-animated";
                                    text = "Running and some warning was informed";
                                    break;

                                    case 7: //Error
                                    btnColor = "#dc3545"; 
                                    btnClass = "btn-danger";
                                    animated = "progress-bar-animated";
                                    text = "Running and some error was informed";
                                    break;
 
                                }
                                return <>
                                            <td className="running-table" style={{ verticalAlign: "bottom" }}>

                                                <div className="d-flex justify-content-center align-items-end" style={{ height: duration.percent*200+'px' , justifyContent: "flex-end"}}>
                                                    <div className={`vr ${btnClass} btn progress-bar progress-bar-striped ${animated}`} 
                                                        style={{justifyContent: "flex-end", opacity: "70%", color: btnColor}}
                                                        title={"Started at: "+duration.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: "+duration.end.replace('T', ' ').slice(0, 24)+"\r\nTotal duration: "+duration.duration/1000+" seconds\r\n\r\nInitialized"}>
                                                    </div> 
                                                </div>
                                            </td>
                                </>
                            })
                        }
                        
                    </tr>
                    {
                        Object.keys(tasks).map((key, key1) => {
                            const task = tasks[key];
                            return <tr>
                                        <td key={key1}>{key}</td>
                                        {
                                            task.map((execution, key2) => {
                                                switch (execution.status) {
                                                    //Initialized
                                                    case 1: return <td>
                                                                        <button key={key2} type="button" className="btn task btn-sm btn-info position-relative"
                                                                            title={"Started at: "+execution.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: -"+"\r\n\r\nInitialized"} // Texto do tooltip
                                                                            onClick={e => handleTaskClick(execution.task_id)}
                                                                            > <i className="bi bi-check-lg"></i> Stated 
                                                                        </button>
                                                                    </td>
                                                    break;
                                                    
                                                    //Error
                                                    case 2: return <td>
                                                                        <button type="button" className="btn task btn-sm btn-danger position-relative"
                                                                            title={"Started at: "+execution.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: "+execution.end.replace('T', ' ').slice(0, 24)+"\r\n\r\n"+execution.message}
                                                                            onClick={e => handleTaskClick(execution.task_id)}>
                                                                            <i className="bi bi-check-lg"></i> Error 
                                                                        </button>
                                                                    </td>
                                                    break;

                                                    //Warning
                                                    case 3: return <td>
                                                                        <button type="button" className="btn task btn-sm btn-warning position-relative"
                                                                            title={"Started at: "+execution.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: "+execution.end.replace('T', ' ').slice(0, 24)+"\r\n\r\n"+execution.message}
                                                                            onClick={e => handleTaskClick(execution.task_id)}>
                                                                            <i className="bi bi-check-lg"></i> Warning 
                                                                        </button>
                                                                    </td>
                                                    break;

                                                    //Success
                                                    case 4: return <td>
                                                                        <button type="button" className="btn task btn-sm btn-success position-relative"
                                                                            title={"Started at: "+execution.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: "+execution.end.replace('T', ' ').slice(0, 24)+"\r\n\r\n"+execution.message}
                                                                            onClick={e => handleTaskClick(execution.task_id)}>
                                                                            <i className="bi bi-check-lg"></i> Success 
                                                                        </button>
                                                                    </td>
                                                    break;

                                                    //PartialError
                                                    case 5: return <td>
                                                                        <button type="button" className="btn task btn-sm btn-danger position-relative"
                                                                            title={"Started at: "+execution.start.replace('T', ' ').slice(0, 24)+"\r\nEnded at: "+execution.end.replace('T', ' ').slice(0, 24)+"\r\n\r\n"+execution.message}
                                                                            onClick={e => handleTaskClick(execution.task_id)}>
                                                                            <i className="bi bi-check-lg"></i> Error 
                                                                        </button>
                                                                    </td>
                                                    break;

                                                
                                                    default: return <td></td>
                                                    break;
                                                }
                                            })
                                        }
                                    </tr>
                        })
                    }
                </tbody>
            </table>
 



            <div style={console} id="code">
                <Log id="console" content={consoleContent}></Log>
            </div>
        </>

    );
}
