import { env } from "../env";
import { AxiosHttpClient } from "../utils/http/axios-http-client";
import { HttpClient } from "../utils/http/http-client";
import { ServerResponse } from '../utils/http/server-response';
import { express as stateless } from '@bradesco/coreopen-stateless-js';
import Logger from "../config/Logger";



export class BeneficiosSeguroService {
  constructor(private httpClient: HttpClient = new AxiosHttpClient()) { }

  public async beneficiosSeguro(request: any, headers: object): Promise<ServerResponse> {

    Logger.info("BeneficiosSeguroService.beneficiosSeguro.init");

    const xsd = stateless.getXSD(headers);
    const options = { Accept: "application/json" };
    xsd.exportTo(options);

    const url = `${env.API_BENEFICIOS_SEGURO}` + '/seguros/beneficios';

    Logger.info('URL: ', url);

    return this.httpClient.get(`${url}`, { headers: options })
      .catch(err => Promise.reject(err));
   
  }
}