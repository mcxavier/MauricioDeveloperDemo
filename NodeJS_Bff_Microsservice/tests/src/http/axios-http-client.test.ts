import axios from 'axios';
import { AxiosHttpClient } from '../../../src/utils/http/axios-http-client'
import { HttpClient } from '../../../src/utils/http/http-client'

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Axios Endpoints', () => {
  const MENSAGEM_ERRO = 'Algum erro';

  const urlEsperadaPost = 'https://192.168.0.175/oferta/concessaoCartao';
  const bodyRequest = {};

  const urlEsperada = 'https://192.168.0.175/oferta/concessaoCartao';

  test('Deve validar a chamada e retorno do método get', async () => {
    const res = {};

    const mockPromiseResponse = {
      body: res,
      status: 200,
      headers: { "content-type": "application/json" }
    };

    mockedAxios.get.mockImplementationOnce(() => Promise.resolve({
      status: 200,
      data: res,
      headers: { "content-type": "application/json" }
    }));

    const wrapper: HttpClient = new AxiosHttpClient();
    const result = await wrapper.get(urlEsperada);

    expect(result).toEqual(mockPromiseResponse);
    expect(mockedAxios.get).toHaveBeenCalledTimes(1);
    expect(mockedAxios.get).toHaveBeenLastCalledWith(urlEsperada, undefined);
  });

  test('Deve validar a chamada e retorno do método post', async () => {
    const res = {};

    const mockPromiseResponse = {
      body: res,
      status: 200,
      headers: { "content-type": "application/json" }
    };

    mockedAxios.post.mockImplementationOnce(() => Promise.resolve({
      status: 200,
      data: res,
      headers: { "content-type": "application/json" }
    }));

    const wrapper: HttpClient = new AxiosHttpClient();
    const result = await wrapper.post(urlEsperadaPost, bodyRequest);

    expect(result).toEqual(mockPromiseResponse);
    expect(mockedAxios.post).toHaveBeenCalledTimes(1);
    expect(mockedAxios.post).toHaveBeenLastCalledWith(urlEsperadaPost, bodyRequest, undefined);
  });

  test('Deve validar o erro no retorno do método get', async (done) => {
    const mockPromiseReject = {
      code: 1010,
      message: MENSAGEM_ERRO
    };

    mockedAxios.get.mockImplementationOnce(() => Promise.reject(mockPromiseReject));

    const wrapper: HttpClient = new AxiosHttpClient();
    await wrapper.get(urlEsperada).catch(e => {
      expect(e.body).toBe(MENSAGEM_ERRO);
      expect(e.status).toBe(500);
      done();
    });
  });


  test('Deve validar o erro no retorno do método post', async (done) => {
    const mockPromiseReject = {
      code: 1011,
      message: MENSAGEM_ERRO
    };

    mockedAxios.post.mockImplementationOnce(() => Promise.reject(mockPromiseReject));

    const wrapper: HttpClient = new AxiosHttpClient();
    await wrapper.post(urlEsperada,bodyRequest).catch(e => {
      expect(e.body).toBe(MENSAGEM_ERRO);
      expect(e.status).toBe(500);
      done();
    });
  });
});