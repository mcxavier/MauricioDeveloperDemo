import { AutenticarGatewayService } from '../services/AutenticarGatewayService';
import { ListarCartoesService } from '../services/ListarCartoesService';
import { DadosSeguroService } from '../services/DadosSeguroService';
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import express from "express";
import Logger from '../config/Logger';
import { ServerResponse } from '../utils/http/server-response';
import { Exception } from '../exception/Exception';
import { getXsd } from "../utils/Stetaless";

export default class ListarCartoesController {
    constructor(
        private listarService: ListarCartoesService = new ListarCartoesService(),
        private autenticarService: AutenticarGatewayService = new AutenticarGatewayService(),
        private segurosService: DadosSeguroService = new DadosSeguroService(),
    ) { }

    public listarCartoesSeguro = async (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {
            Logger.info("listarCartoesSeguro.init - Controller", request);
            
            const xsd = getXsd(response, request);
            Logger.info("XSD: ", request, xsd);
            
            const autenticacaoGateway: ServerResponse = await this.autenticarService.autenticarGatewayCartoes(request, response);
            Logger.info("Status autenticacao: ", request, autenticacaoGateway.status);
            Logger.info("Response autenticacao: ", request, autenticacaoGateway.body);
            
            const cartoesCliente: ServerResponse = await this.listarService.listarCartoesSeguro(request, response);
            Logger.info("Response Listar Cartoes: ", request, cartoesCliente.body);
            Logger.info("Response Listar Cartões listarCartoesSeguro - controller: ", cartoesCliente);
            Logger.log(request);
            response.status(cartoesCliente.status).json(cartoesCliente.body);

        } catch (err) {
            Logger.error(err.body, request);
            next(new Exception(err.status, err.body));
        }
    }
}
