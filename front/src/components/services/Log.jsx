import React, { useState, useEffect } from 'react';
import Api from './Api';

export default function Log({container_id}){

    const [logs, setLogs] = useState([]);
    useEffect(() => {
        
        Api.get("/Container/Logs/"+container_id).then(function(response) 
        {
            //console.log(response.data);
            setLogs(response.data);
        })
        .catch((error) => {
                throw new Error("Erro ao chamar a API: "+error);
            }
        );
    }, []);

    if(logs.length == 0){
        return (
            <>
                
            </>
        )
    }

    return (
        <>
          {logs.length}
            {logs.map((log) => (
              <>{log.info}<br /></>
            ))}
        </>
      );
    
}