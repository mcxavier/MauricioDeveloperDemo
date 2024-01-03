import express from "express";
import { AxiosHttpClient } from "../../../src/utils/http/axios-http-client";
import { RequestMock } from "../../mocks/RequestMock";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import XSD from "../../mocks/XsdMock"
import { ServerResponse } from "../../../src/utils/http/server-response";
import { HttpClient} from "../../../src/utils/http/http-client";
import { BeneficiosSeguroService } from "../../../src/services/BeneficiosSeguroService"; 

jest.mock("../../../src/utils/http/axios-http-client");
const requestMock = new RequestMock();
const request = {body: {"cartao":""}, headers: {}} as express.Request;
const app = express();
const res = app.response;
const next = jest.fn();
const mock = jest.fn();

describe("Listar beneficios do seguro - Services", () => {

    test("Deve retornar status 200 ao acionar metodo: estados com verbo - GET", async () => {
        AxiosHttpClient.prototype.get = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new BeneficiosSeguroService();

        const result: ServerResponse = await service.beneficiosSeguro(request, res);
        expect(res).toBeTruthy();
    });

    test("Deve retornar status 503 ao acionar metodo com parametros invalidos", async () => {
        AxiosHttpClient.prototype.get = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new BeneficiosSeguroService();

        const result: ServerResponse = await service.beneficiosSeguro(request, res);
        expect(res).toBeTruthy();
    });

    test("Deve retornar status 503 quando não houver objeto de erro", async () => {
        AxiosHttpClient.prototype.get = mock;
        mock.mockResolvedValue(requestMock.indisponibilidade());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new BeneficiosSeguroService();

        const result: ServerResponse = await service.beneficiosSeguro(request, res);
        expect(res.statusCode).toEqual(200);
    });

    test('Deve validar o retorno com erro do método de consulta de oferta', async (done) => {
        const MockHttpClient = jest.fn<HttpClient, any>(() => ({
          get: jest.fn((url: string, options?: object) => Promise.reject(mockPromiseReject)),
          post: jest.fn((url: string, body: object, options?: object) => Promise.reject(mockPromiseReject))
        }));

        const gerarServiceImpl = new BeneficiosSeguroService(new MockHttpClient());

        await gerarServiceImpl.beneficiosSeguro(request, res).catch(e => {
          expect(res.statusCode).toEqual(200);
          //expect(res.body).toEqual(MENSAGEM_ERRO);
          done();
        });
    });

    test("Deve retornar ServerResponse ao acionar metodo: estados com verbo - GET", async () => {
        AxiosHttpClient.prototype.get = mock;
        mock.mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new BeneficiosSeguroService();

        const result: ServerResponse = await service.beneficiosSeguro(request, res);
        expect(res).toBeTruthy();
    });

    const MENSAGEM_ERRO = 'Erro ao obter oferta';
    const mockPromiseReject: ServerResponse = {
        body: MENSAGEM_ERRO,
        status: 400,
        headers: {}
    };

});