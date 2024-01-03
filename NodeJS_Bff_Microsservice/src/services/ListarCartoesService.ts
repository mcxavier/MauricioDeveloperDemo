import { AxiosHttpClient } from '../utils/http/axios-http-client';
import { DadosSeguroService } from "../services/DadosSeguroService";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import Logger from "../config/Logger";
import { ServerResponse } from '../utils/http/server-response';
import { env } from "../env";
import { HttpClient } from '../utils/http/http-client';

class CartoesSegurosResponse {
    nomeProduto: string;
    valorMensal: string;
    cobranca: string;
    cartoes : [CartaoSeguro]
}

class CartaoSeguro {
    codigoPlastico: string;
    produtoPrincipal: string;
    nomeEmbosso: string;
    numeroCartao: string;
    validadePlastico: string;
    melhorDiaCompra: string;
    formaPagamento: string;
    temFaturaImpressa: string;
    bandeira: string;
    titularAdicional: string;
    nomeCompleto: string;
    contaCartao: string;
    hashCartao: string;
    seguroContratado: string;
}

export class ListarCartoesService {
    constructor(
        private httpClient: HttpClient = new AxiosHttpClient(),
        private cartoesSeguroService: DadosSeguroService = new DadosSeguroService()
    ) { }

    public async listarCartoesSeguro(request: any, response: object): Promise<ServerResponse> {
        Logger.info("listarCartoesCliente.init - Service", request);
        const headers = { "Content-Type": "application/json" };
        
        const xsd = stateless.getXSD(response);
        Logger.info("XSD: ", request, xsd);
        xsd.exportTo(headers);
        Logger.info("Headers: ", request, headers);
        const urlSrvListarCartoesCliente = env.URL_SRV_LISTAR_CARTOES_CLIENTE + "/listar-cartoes-elegiveis";

        Logger.info("URL Listar Cartoes:", request, urlSrvListarCartoesCliente);
        const cpf: string = xsd.open.cliente.cpfCnpj.replace("0000", "");

        const body = {
            processadoras: ["b2k"],
            funcionalidades: [28],
            cpfCnpj: cpf
        };

        let respService = await this.httpClient.post(urlSrvListarCartoesCliente, body, { headers });
        Logger.info("Retorno Servico:", request, respService);

        return Promise.resolve(respService).then(async ({status, headers, body}) => {            
            return {status, headers, body: await this.completarComFlagSeguro(headers, body)};
        });
    }

    private completarComFlagSeguro(headers, responseBody): any {
        Logger.info("CompletarcomFlagSeguro.init:", headers, responseBody);
        const promises = responseBody.cartoes.map(async (cartao): Promise<CartaoSeguro> => { 
            Logger.info("Chamando o serviço dadosSeguros", cartao);
            const {body} = await this.cartoesSeguroService.dadosSeguro({"body": {"hashCartao": cartao.hashCartao}}, headers);
            Logger.info("Resposta serviço dadosSeguros", body);
            return {...cartao, "seguroContratado": body.flagSeguro || "N"};
        });

        return Promise.all(promises);
    }
}