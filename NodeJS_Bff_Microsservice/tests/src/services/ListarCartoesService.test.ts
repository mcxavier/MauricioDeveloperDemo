import { ListarCartoesService } from '../../../src/services/ListarCartoesService';
import express from "express";
import { AxiosHttpClient } from "../../../src/utils/http/axios-http-client";
import { RequestMock } from "../../mocks/RequestMock";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import XSD from "../../mocks/XsdMock"
import { ServerResponse } from "../../../src/utils/http/server-response";
import { HttpClient} from "../../../src/utils/http/http-client";
import { any } from 'async';
import {CodigoRetorno} from "../../../src/constants/CodigoRetorno";


jest.mock("../../../src/utils/http/axios-http-client");
const requestMock = new RequestMock();
const request = {body: {"cartao":""}, headers: {}} as express.Request;
const app = express();
const res = app.response;
const next = jest.fn();
const mock = jest.fn();

describe("Listar Cartões - Services", () => {

    test("Teste unitário - POST", async () => {
        AxiosHttpClient.prototype.post = mock;
        mock.mockResolvedValue( {status: CodigoRetorno.OK, body: {"cartoes":[{"numeroCartao" : "1234"}]}});

        jest.spyOn(stateless, "getXSD").mockImplementation(() => {
            return XSD;
        });

        const service = new ListarCartoesService();

        const result: ServerResponse = await service.listarCartoesSeguro(request, res);
        expect(res).toBeTruthy();
    });

    const MENSAGEM_ERRO = 'Erro ao obter oferta';
    const mockPromiseReject: ServerResponse = {
        body: MENSAGEM_ERRO,
        status: 400,
        headers: {}
    };

});
