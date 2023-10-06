import react, { useEffect, useState }from "react";
import Log from "../services/Log";
import { useParams } from "react-router-dom";
import Api from '../services/Api';

import Popper from 'popper.js';


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

 



    return (

        <>
            <table className="table table-bordered table-sm">
                <thead>
                    Tempo total
                </thead>
                <tbody>
                        {
                            Object.keys(tasks).map((key) => {
                                const task = tasks[key];
                                return <tr>
                                            <td>{key}</td>
                                            {
                                                task.map((execution) => {
                                                    switch (execution.status) {
                                                        case 1: return <td><button type="button" className="btn task btn-sm btn-primary position-relative"
                                                                                data-toggle="tooltip" // Indica que este botão terá um tooltip
                                                                                data-placement="top" // Posição do tooltip (pode ser top, bottom, left ou right)
                                                                                data-html="true" 
                                                                                title={"Inicio: "+execution.start+"  Fim: "+execution.end} // Texto do tooltip
                                                                                > <i className="bi bi-check-lg"></i> OK </button></td>
                                                        break;
                                                        
                                                        case 2: return <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> OK </button></td>
                                                        break;

                                                        case 3: return <td><button type="button" className="btn task btn-sm btn-warnning position-relative"> <i className="bi bi-check-lg"></i> OK </button></td>
                                                        break;

                                                        case 4: return <td><button type="button" className="btn task btn-sm btn-info position-relative"> <i className="bi bi-check-lg"></i> OK </button></td>
                                                        break;

                                                        case 5: return <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-check-lg"></i> OK </button></td>
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
            <table className="table table-bordered table-sm">
                <tbody>
                    <tr>
                        
                        <td className="running-table">Tempo total</td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                
                                <div className="vr btn btn-success progress-bar progress-bar running"></div>
                            </div>
                        </td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div>
                        </td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td className="running-table">
                            <div className="d-flex" style={{ height: "20px" }}>
                                <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                            </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                        <td ><div className="d-flex" style={{ height: "20px" }}>
                            <div className="vr btn btn-success progress-bar progress-bar-striped progress-bar-animated running"></div>
                        </div></td>
                    </tr>
                    <tr>
                        <td className="running-table">Task 1</td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-success position-relative"> <i className="bi bi-check-lg"></i> </button></td>
                    </tr>
                    <tr>
                        <td className="running-table">Task 2</td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                        <td><button type="button" className="btn task btn-sm btn-danger position-relative"> <i className="bi bi-exclamation-triangle-fill"></i> </button></td>
                    </tr>

                </tbody>
            </table>



            <div style={console} id="code">
                <Log container_id={container_id}></Log>
            </div>
        </>

    );
}
