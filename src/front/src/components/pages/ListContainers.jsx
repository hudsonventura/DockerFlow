import React, {useEffect, useState} from 'react';
import { Link } from "react-router-dom";
import Api from '../services/Api';




export default function ListContainers() {
    
    
    const [containers, setContainers] = useState([]);
    useEffect(() => {

        Api.get("/Container").then(function (response) {
            console.log(response.data);
            setContainers(response.data);
        })
            .catch((error) => {
                throw new Error("Erro ao chamar a API: " + error);
            }
            );
    }, []);

    if (containers.length == 0) {
        return (
            <>
                Vazio
            </>
        )
    }


    return (

        <>
            <table className="table table-striped">
            <thead>
                <tr>
                <th scope="col">ID</th>
                <th scope="col">Service</th>
                <th scope="col">Status</th>
                </tr>
            </thead>
            <tbody>
                {containers.map((container) => (
                    <tr key={container.containerID}>
                        <th scope="row">{container.containerID.substring(0, 9)}</th>
                        <td><Link to={"/Container/"+container.containerID}>{container.serviceName}</Link></td>
                        <td>{container.status}</td>
                    </tr>
                ))}
            </tbody>
            </table>
        </>

    );
}
