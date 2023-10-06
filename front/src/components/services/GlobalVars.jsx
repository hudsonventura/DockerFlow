import { createContext, useEffect } from "react"; 
import Configs from './Configs';

export const GlobalContext = createContext();
//export default GlobalContext;


export const GlobalVarsProvider = ({children}) => {
    return(
        <GlobalContext.Provider value={GlobalVars}>
            {children}
        </GlobalContext.Provider>
    );
}




const getLogedUser = () => {
    const userJson = sessionStorage.getItem('user');
    const user = JSON.parse(userJson);
    if(user == null){
        //user.name = 'Usuário nao logado';
    }
    return user
}

const setLogedUser = (user) => {
    if(!user){
        sessionStorage.removeItem('user');
        return;
    }  

    //console.log('Setando usuario logado: '+user);
    sessionStorage.setItem('user', JSON.stringify(user));
}


const getToken = () => {
    const tokenString = sessionStorage.getItem('token');
    const userToken = JSON.parse(tokenString);
    return userToken
}

const setToken = (userToken) => {
    if(!userToken){
        sessionStorage.removeItem('token');
        return;
    }  

    //console.log('Setando token: '+userToken);
    sessionStorage.setItem('token', JSON.stringify(userToken));
}

function SetHTMLTitle(titulo){
    const configs = Configs();

    let gerado = titulo + " - " + configs.tituloSistema;

    useEffect(() => {
        document.title = gerado
    }, []);
}

export const GlobalVars = {
    getConfigs: Configs,
    getToken, 
    setToken,
    SetHTMLTitle,
    getLogedUser,
    setLogedUser
};

