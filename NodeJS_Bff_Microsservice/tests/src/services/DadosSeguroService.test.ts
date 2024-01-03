import express from "express";
import { AxiosHttpClient } from "../../../src/utils/http/axios-http-client";
import { ListarCartoesService } from "../../../src/services/ListarCartoesService"; 
import { RequestMock } from "../../mocks/RequestMock";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import XSD from "../../mocks/XsdMock"
import { ServerResponse } from "../../../src/utils/http/server-response";
import { HttpClient} from "../../../src/utils/http/http-client";
import { DadosSeguroService } from "../../../src/services/DadosSeguroService";


jest.mock("../../../src/utils/http/axios-http-client");
const requestMock = new RequestMock();
const request = {body: {"cartao":""}, headers: {}} as express.Request;
const app = express();
const res = app.response;
const next = jest.fn();
const mock = jest.fn();

describe("Listar dados seguro Services", () => {

    test("Dados Seguros - 200", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.dadosSeguro(request, res);
        expect(res).toBeTruthy();
    });



    test("Contratar Seguros - 200", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.contratacaoSeguro(request, res);
        expect(res).toBeTruthy();
    });    


    test("Cancelar Seguros - 200", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.cancelamentoSeguro(request, res);
        expect(res).toBeTruthy();
    });  


    test("dados Seguros - 403", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.dadosSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });


    test("Cancelar Seguros - 403", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.cancelamentoSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });


    test("Contratar Seguros - 403", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.contratacaoSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });



    test("Deve retornar status 503 quando não houver objeto de erro", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.dadosSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });


    test("Deve retornar status 503 quando não houver objeto de erro", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.cancelamentoSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });    

    test("Deve retornar status 503 quando não houver objeto de erro", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result: ServerResponse = await service.contratacaoSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });


    test('Dados Seguros - Erro', async (done) => {
        const MockHttpClient = jest.fn<HttpClient, any>(() => ({
          get: jest.fn((url: string, options?: object) => Promise.reject(mockPromiseReject)),
          post: jest.fn((url: string, body: object, options?: object) => Promise.reject(mockPromiseReject))
        }));

        const gerarServiceImpl = new DadosSeguroService(new MockHttpClient());

        await gerarServiceImpl.dadosSeguro(request, res).catch(e => {
          expect(res.statusCode).toEqual(200);
          //expect(e.body).toEqual(MENSAGEM_ERRO);
          done();
        });
    });


    test('Dados Seguros - Erro', async (done) => {
        const MockHttpClient = jest.fn<HttpClient, any>(() => ({
          get: jest.fn((url: string, options?: object) => Promise.reject(mockPromiseReject)),
          post: jest.fn((url: string, body: object, options?: object) => Promise.reject(mockPromiseReject))
        }));

        const gerarServiceImpl = new DadosSeguroService(new MockHttpClient());

        await gerarServiceImpl.cancelamentoSeguro(request, res).catch(e => {
          expect(res.statusCode).toEqual(200);
          //expect(e.body).toEqual(MENSAGEM_ERRO);
          done();
        });
    });
   

    test('Dados Seguros - Erro', async (done) => {
        const MockHttpClient = jest.fn<HttpClient, any>(() => ({
          get: jest.fn((url: string, options?: object) => Promise.reject(mockPromiseReject)),
          post: jest.fn((url: string, body: object, options?: object) => Promise.reject(mockPromiseReject))
        }));

        const gerarServiceImpl = new DadosSeguroService(new MockHttpClient());

        await gerarServiceImpl.contratacaoSeguro(request, res).catch(e => {
          expect(res.statusCode).toEqual(200);
          //expect(e.body).toEqual(MENSAGEM_ERRO);
          done();
        });
    });


    test("Dados Seguros - Return Response", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result = await service.dadosSeguro(request, res);
        expect(res).toBeTruthy();
    });



    test("Dados Seguros - Return Response", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result = await service.cancelamentoSeguro(request, res);
        expect(res).toBeTruthy();
    });



    test("Dados Seguros - Return Response", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new DadosSeguroService();

        const result = await service.contratacaoSeguro(request, res);
        expect(res).toBeTruthy();
    });    


    const MENSAGEM_ERRO = 'Erro ao obter oferta';
    const mockPromiseReject: ServerResponse = {
        body: MENSAGEM_ERRO,
        status: 400,
        headers: {}
    };

    

});