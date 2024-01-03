import { env } from "../env";
import { AxiosHttpClient } from "../utils/http/axios-http-client";
import { HttpClient } from "../utils/http/http-client";
import { ServerResponse } from '../utils/http/server-response';
import { express as stateless } from '@bradesco/coreopen-stateless-js';
import Logger from "../config/Logger";


export class DadosSeguroService {
  constructor(private httpClient: HttpClient = new AxiosHttpClient()) { }
  
  public async dadosSeguro(request: any, headers: object): Promise<ServerResponse> {

    Logger.info("DadosSeguroService.dadosSeguro.init");
    Logger.info("DadosSeguroService.init - Service", request);

    Logger.info("Headers: ", request, headers);
    const xsd = stateless.getXSD(headers);
    Logger.info("XSD: ", request, xsd);
    const options = { "Content-Type": "application/json" };  
    xsd.exportTo(options);

    const url = `${env.API_BENEFICIOS_SEGURO}` + '/seguros/listar';
    Logger.info("URL Listar Seguro:", request, url);

    const body = {
      hashCartao: request.body["hashCartao"]
    };      

    return this.httpClient.post(`${url}`, body, { headers: options })
      .catch(err => Promise.reject(err));
      
  }

  public async contratacaoSeguro(request: any, headers: object): Promise<ServerResponse> {

    Logger.info("DadosSeguroService.contratacaoSeguro.init");
    Logger.info("DadosSeguroService.init - Service", request);

    Logger.info("Headers: ", request, headers);
    const xsd = stateless.getXSD(headers);
    Logger.info("XSD: ", request, xsd);
    const options = { Accept: "application/json" };
    xsd.exportTo(options);

    const url = `${env.API_BENEFICIOS_SEGURO}` + '/seguros/contratar';
    Logger.info("URL Contratar Seguro:", request, url);  

    const body = {
      hashCartao: request.body["hashCartao"],
      tokenTransacao: request.body["tokenTransacao"],
      dadosAutorizador: request.body["dadosAutorizador"]
    };    
    
    return this.httpClient.post(`${url}`, body, { headers: options })
      .catch(err => Promise.reject(err));
      
  }

  public async cancelamentoSeguro(request: any, headers: object): Promise<ServerResponse> {

    Logger.info("DadosSeguroService.cancelamentoSeguro.init");
    Logger.info("DadosSeguroService.init - Service", request);

    const xsd = stateless.getXSD(headers);
    Logger.info("Headers: ", request, headers);
    Logger.info("XSD: ", request, xsd);

    const options = { Accept: "application/json" };
    xsd.exportTo(options);

    const url = `${env.API_BENEFICIOS_SEGURO}` + '/seguros/cancelar';
    Logger.info("URL Cancelar Seguro:", request, url);

    const body = {
      hashCartao: request.body["hashCartao"],
      tokenTransacao: request.body["tokenTransacao"],
      dadosAutorizador: request.body["dadosAutorizador"]
    };    

    return this.httpClient.post(`${url}`, body, { headers: options })
      .catch(err => Promise.reject(err));
      
  }
    
}
