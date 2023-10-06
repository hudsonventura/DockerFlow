import axios from "axios";
import Configs from './Configs';

import { GlobalVars as GLOBALS } from "./GlobalVars";

const configs = Configs();



let token = GLOBALS.getToken();

const headers = {
    "Access-Control-Allow-Origin": "*",
    "Accept":"application/json"
};


const Api = axios.create({
  baseURL: configs['Backend']['url'],
  headers: headers
});

export const apiAnonimous = axios.create({
    baseURL: configs['Backend']['url']
  });

export default Api;