import { express as stateless } from "@bradesco/coreopen-stateless-js";
import * as express from 'express';
import { AxiosHttpClient } from '../utils/http/axios-http-client';
import { HttpClient } from "../utils/http/http-client";
import Logger from "../config/Logger";
import { env } from "../env";

export class AutenticarGatewayService {
    constructor(private httpClient: HttpClient = new AxiosHttpClient()) { }

    public async autenticarGatewayCartoes(request: express.Request, response: express.Response) {
        Logger.info("autenticarGatewayCartoes.init - Service", request);
        let headers = { "Content-Type": "application/json" };
        const xsd = stateless.getXSD(response);
        Logger.info("XSD: ", request, xsd);
        xsd.exportTo(headers);
        Logger.info("Headers: ", request, headers);

        const urlAutenticarGatewayCartoes = env.URL_SRV_GATEWAY_CARTOES_AUTENTICAR + "/autenticarGatewayCartoes";
        Logger.info("URL: ", urlAutenticarGatewayCartoes);

        return this.httpClient.get(urlAutenticarGatewayCartoes, { headers })
        .catch(err => Promise.reject(err));
    }
}
