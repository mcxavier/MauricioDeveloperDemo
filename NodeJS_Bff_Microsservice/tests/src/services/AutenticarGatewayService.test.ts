import express from 'express';
import { AxiosHttpClient } from "../../../src/utils/http/axios-http-client";
import { RequestMock } from "../../mocks/RequestMock";
import { express as stateless } from '@bradesco/coreopen-stateless-js';
import XsdMock from "../../mocks/XsdMock";
import { AutenticarGatewayService } from "../../../src/services/AutenticarGatewayService";
import { HttpClient } from '../../../src/utils/http/http-client';

const requestMock = new RequestMock();
const request = { headers: {} } as express.Request;
const response = express().response;

describe("AutenticarGatewayService", () => {
    test("Deve retornar autenticação quando chamar Service", async () => {
        AxiosHttpClient.prototype.get = jest.fn().mockResolvedValue(requestMock.success());

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XsdMock;
        })

        const result = await new AutenticarGatewayService().autenticarGatewayCartoes(request, response);

        expect(response).toBeTruthy();
    })

    test('Deve validar o retorno com erro ao chamar service', async () => {
        const MockHttpClient = jest.fn<HttpClient, any>(() => ({
          get: jest.fn((url: string, options?: object) => Promise.reject(requestMock.pedidoRuim())),
          post: jest.fn((url: string, body: object, options?: object) => Promise.reject(requestMock.error()))
        }));

        const service = new AutenticarGatewayService(new MockHttpClient());

        await service.autenticarGatewayCartoes(request, response).catch(e => {
          expect(response.statusCode).toEqual(200);
        });
    });
})