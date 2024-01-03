/**
 * Classe: WrapperHttpClient
 * Função: Prover um 'interseptor' para as requisição HTML, para geração de Logs
 */

import axios, { AxiosResponse } from "axios";
import Logger from '../config/Logger';
import { ServerResponse } from "../utils/http/server-response";
import WrapperHttpClient from "./WrapperHttpClient";

export default class WrapperAxios implements WrapperHttpClient {

  public async get(url: string, config?: object) {
    try {
      const getResponse = await axios.get(url, config);
      return this.buildResponse(getResponse);
    } catch (error) {
      Logger.error(error);
      throw Promise.reject(this.buildResponse(error.response));
    }
  }

  public async post(url: string, data?: object, config?: object) {
    try {
      const postResponse = await axios.post(url, data, config);
      return this.buildResponse(postResponse);
    } catch (error) {
      Logger.error(error);
      throw Promise.reject(this.buildResponse(error.response));
    }
  }

  public async put(url: string, data?: object, config?: object) {
    try {
      const putResponse = await axios.put(url, data, config);
      return this.buildResponse(putResponse);
    } catch (error) {
      Logger.error(error);
      throw Promise.reject(this.buildResponse(error.response));
    }
  }

  public async delete(url: string, content?: object, config?: object) {
    try {
      const postResponse = await axios.delete(url, { data: content, headers: config as any });
      return this.buildResponse(postResponse);
    } catch (error) {
      Logger.error(error);
      throw Promise.reject(this.buildResponse(error.response));
    }
  }

  private buildResponse = (axiosResponse: AxiosResponse) => {
    return {
      status: axiosResponse.status || 500,
      body: axiosResponse.statusText,
      headers: axiosResponse.data
    } as ServerResponse;
  }
}
