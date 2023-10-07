export default function Configs(){
    let json = '';
    try {
        json = require('../../appsettings.json'); 
    } catch (error) {
        //console.log("Crie o arquivo config.json na pasta 'src'");
        return null;
    }
    
    return json;
}

