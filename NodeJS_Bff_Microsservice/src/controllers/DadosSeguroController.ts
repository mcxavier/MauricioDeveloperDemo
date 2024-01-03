import { DadosSeguroService } from "../services/DadosSeguroService";
import { AutenticarGatewayService } from '../services/AutenticarGatewayService';
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import express from "express";
import { Mcent } from "../utils/Mcent";
import Logger from '../config/Logger';
import { ServerResponse } from '../utils/http/server-response';
import { Exception } from '../exception/Exception';
import { getXsd } from "../utils/Stetaless";


export default class DadosSeguroController {

    CODFUNCIONALIDADECONTRATAR = 10;
    CODFUNCIONALIDADECANCELAR = 11;

    constructor(
        private service: DadosSeguroService = new DadosSeguroService(),
        private mcentService: Mcent = new Mcent(),
        private autenticarService: AutenticarGatewayService = new AutenticarGatewayService()
    ) { }

    /**
     * @swagger
     * /health:
     *    get:
     *      summary: Retornar dados do seguro.
     *      responses:
     *        200:
     *          description: OK
     *          schema:
     *             type: object
     *             properties:
     *                name:
     *                   type: string
     *                   example: bcpf-cartoes-seguros
     *                status:
     *                   type: string
     *                   example: available
     */
    public listaDadosSeguro = async (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {
            const xsd = getXsd(response, request);

            Logger.info("XSD: ", request, xsd);
            const autenticacaoGateway: ServerResponse = await this.autenticarService.autenticarGatewayCartoes(request, response);
            Logger.info("Status autenticacao: ", request, autenticacaoGateway.status);
            Logger.info("Response autenticacao: ", request, autenticacaoGateway.body);

            const dadosSeguro = await this.service.dadosSeguro(request, response).catch((error) => error);

            Logger.info("Retorno Service - DadosSeguroBusiness.dadosSeguro: ", dadosSeguro);

            response.status(dadosSeguro.status).json(dadosSeguro.body);
        } catch (err) {
            next(new Exception(err.status, err.body));
        }
    }

    /**
     * @swagger
     * /health:
     *    get:
     *      summary: Realiza a contratação de um seguro.
     *      responses:
     *        200:
     *          description: OK
     *          schema:
     *             type: object
     *             properties:
     *                name:
     *                   type: string
     *                   example: bcpf-cartoes-seguros
     *                status:
     *                   type: string
     *                   example: available
     */    
    public contratacaoSeguro = async (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {

            const xsd = getXsd(response, request);
            Logger.info("XSD: ", request, xsd);

            const autenticacaoGateway: ServerResponse = await this.autenticarService.autenticarGatewayCartoes(request, response);
            Logger.info("Status autenticacao: ", request, autenticacaoGateway.status);
            Logger.info("Response autenticacao: ", request, autenticacaoGateway.body);
   
            Logger.info("DadosSeguroBusiness.contratacaoSeguro.init"); 

            //CHAMADA COM ID 1 - ANTES DA TRANSAÇÃO

            Logger.info("AssinarProposta - antes do mcent - 1");
            this.mcentService.gravarMcent(request, response, 1, this.CODFUNCIONALIDADECONTRATAR);
            Logger.info("AssinarProposta - depois do mcent - 1");

            const seguro = await this.service.contratacaoSeguro(request, response).catch((error) => error);

            Logger.info("Dados seguros controller - retorno chamada contratacaoSeguro: ", seguro)

            request.body.dadosTransacao = JSON.stringify(seguro.body);

            Logger.info("AssinarProposta - antes do mcent - 2");
            this.mcentService.gravarMcent(request, response, 2, this.CODFUNCIONALIDADECONTRATAR);
            Logger.info("AssinarProposta - depois do mcent - 2");
            
            //CHAMADA COM ID 2 - DEPOIS DA TRANSAÇÃO

            Logger.info("Retorno Service - DadosSeguroBusiness.contratacaoSeguro: ", request, autenticacaoGateway.status);

            response.status(seguro.status).json(seguro.body);
        } catch (err) {
            next(new Exception(err.status, err.body));
        }
    }
    

     /**
     * @swagger
     * /health:
     *    get:
     *      summary: Realiza o cancelamento de um seguro.
     *      responses:
     *        200:
     *          description: OK
     *          schema:
     *             type: object
     *             properties:
     *                name:
     *                   type: string
     *                   example: bcpf-cartoes-seguros
     *                status:
     *                   type: string
     *                   example: available
     */   
    public cancelamentoSeguro = async (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {

            const xsd = getXsd(response, request);
            Logger.info("XSD: ", request, xsd);
            
            const autenticacaoGateway: ServerResponse = await this.autenticarService.autenticarGatewayCartoes(request, response);
            Logger.info("Status autenticacao: ", request, autenticacaoGateway.status);
            Logger.info("Response autenticacao: ", request, autenticacaoGateway.body);
        
            Logger.info("DadosSeguroBusiness.cancelamentoSeguro.init"); 

            Logger.info("AssinarProposta - antes do mcent - 1");
            this.mcentService.gravarMcent(request, response, 1, this.CODFUNCIONALIDADECANCELAR);
            Logger.info("AssinarProposta - depois do mcent - 1");

            const seguro = await this.service.cancelamentoSeguro(request, response).catch((error) => error);

            Logger.info("Dados seguros controller - retorno chamada cancelamentoSeguro: ", seguro)

            request.body.dadosTransacao = JSON.stringify(seguro.body);

            Logger.info("AssinarProposta - antes do mcent - 2");
            this.mcentService.gravarMcent(request, response, 2, this.CODFUNCIONALIDADECANCELAR);
            Logger.info("AssinarProposta - depois do mcent - 2");

            Logger.info("Retorno Service - DadosSeguroBusiness.cancelamentoSeguro: ", seguro);

            response.status(seguro.status).json(seguro.body);
        } catch (err) {
            next(new Exception(err.status, err.body));
        }
    }

}