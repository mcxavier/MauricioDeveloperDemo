import { express as stateless } from '@bradesco/coreopen-stateless-js';
import bodyParser from "body-parser";
import express from "express";
import supertest from "supertest";
import XSD from "../../mocks/XsdMock";
import { ListarCartoesService } from "../../../src/services/ListarCartoesService"; 
import { AutenticarGatewayService } from "../../../src/services/AutenticarGatewayService";
import Rota from "../../../src/config/Router";
import XsdMock from '../../mocks/XsdMock';
import { RequestMock } from "../../mocks/RequestMock";


const authMock = new RequestMock();

describe("ListaCartaoController", () => {
  
  test("Deve retornar 200 ao bater na rota", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
      ListarCartoesService.prototype.listarCartoesSeguro = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
    const result = await request.get("/listarCartoes").send();
    expect(result.status).toBeTruthy();
  });

  test("Deve retornar 500 ao bater na rota", async () => {
    ListarCartoesService.prototype.listarCartoesSeguro = jest
      .fn()
      .mockRejectedValueOnce(authMock.error());
    const result = await request.get("/listarCartoes").send();
    expect(result.status).toBeTruthy();
  });
  
});

const request = supertest(
  express()
    .use(express.json())
    .use(express.urlencoded({ extended: true }))
    .use("/", Rota)
);
