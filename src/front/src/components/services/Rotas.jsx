import { React, useContext } from "react";
import { Route, Routes, useParams } from 'react-router-dom';



import ListContainers from '../pages/ListContainers';
import Container from '../pages/Container';
//import Page404 from '../pages/Page404';
//import Page403 from '../pages/Page403';
import { GlobalContext } from "./GlobalVars";




export default function Rotas() {

    let GLOBALS = useContext(GlobalContext);
    console.log(GLOBALS);
    //const token = GLOBALS.getToken();
    //console.log("Router token: " + token);

    //const paginaComumNaoLogado = <Login titulo="Fazer login" />;
    //const paginaAcessoProibido = <Page403 titulo="Acesso proibido. Nana nina não, pidão!" />;



    return (
        
        <Routes>
            <Route path="/" element={<ListContainers />} exact={true} />
            <Route path="/Containers" element={<ListContainers />} exact={true} />
            <Route path="/Container/:container_id" element={<Container />} exact={true} />
        </Routes>
    );



    


}
