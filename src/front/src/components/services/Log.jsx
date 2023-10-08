import React, { useState, useEffect } from 'react';
import Api from './Api';

export default function Log({content}){

    if(content.length == 0){
        return (
            <>
                No content
            </>
        )
    }

    return (
        <>
          {content.map((log, key) => {
            let customOutput;
            
            switch (log.type) {
                case 1: //stared
                    customOutput = <div>-----------------------------------------------------------------------<br />{log.timestamp.replace('T', ' ').slice(0, 24) + " > START"}</div>;
                    break;
                case 2: //ErrorEnd
                    customOutput = <><div style={{color: "red"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " > END "+log.info}</div>-----------------------------------------------------------------------</>;
                    break;
                case 3: //WarnningEnd
                    customOutput = <><div style={{color: "yellow"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " > END "+log.info}</div>-----------------------------------------------------------------------</>;
                    break;
                case 4: //SuccessEnd
                    customOutput = <><div style={{color: "green"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " > END "+log.info}</div>-----------------------------------------------------------------------</>;
                    break;
                case 5: //Info
                    customOutput = <div style={{color: "cyan"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " - "+log.info}</div>;
                    break;
                case 6: //Warning
                    customOutput = <div style={{color: "yellow"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " - "+log.info}</div>;
                    break;
                case 7: //Error
                    customOutput = <div style={{color: "red"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " - "+log.info}</div>;
                    break;
                case 8: //Debug
                    customOutput = <div style={{color: "gray"}}>{log.timestamp.replace('T', ' ').slice(0, 24) + " - "+log.info}</div>;
                    break;
                default: 
                    customOutput = <div>{log.timestamp.replace('T', ' ').slice(0, 24) + " - "+log.info}</div>;
                    break;
              
            }
    
            return (
              <div key={key}>
                {customOutput}
              </div>
            );
          })}
        </>
    );
    
}