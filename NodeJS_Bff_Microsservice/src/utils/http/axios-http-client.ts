import axios, { AxiosResponse } from 'axios';

import { HttpClient } from './http-client';
import { ServerResponse } from './server-response';
import { env } from '../../env';


export class AxiosHttpClient implements HttpClient {

  constructor() {
    // Desabilitado para testes em TH
    // axios.defaults.timeout = Number(env.axiosTimeout);
    axios.defaults.timeout = 0;
  }

  public async get(url: string, options?: object): Promise<ServerResponse> {
    return axios.get(url, options)
      .then(response => this.buildResponse(response))
      .catch(err => Promise.reject(this.buildResponse(err.response || err)));
  }

  public async post(url: string, body: object, options?: object): Promise<ServerResponse> {
    return axios.post(url, body, options)
      .then(response => this.buildResponse(response))
      .catch(err => Promise.reject(this.buildResponse(err.response || err)));
  }

  private buildResponse = ( {status, data, headers, message = "Erro Infra em request axios"} : any ): ServerResponse => ({
    status:   status  || 500,
    body:     data    || message,
    headers:  headers || {}
  })
}