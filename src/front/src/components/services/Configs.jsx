export default function Configs(){
    let json = '';
    try {
        json = require('../../appsettings.json'); 
        //console.log("Aqui");
        //console.log(json);
    } catch (error) {
        //console.log("Crie o arquivo config.json na pasta 'src'");
        return null;
    }
    
    return json;
}

