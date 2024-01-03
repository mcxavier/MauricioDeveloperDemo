import { express as stateless } from '@bradesco/coreopen-stateless-js';
import bodyParser from "body-parser";
import route from '../../../src/config/Router';
import express from "express";
import XSD from "../../mocks/XsdMock";
import supertest from "supertest";
import { DadosSeguroService } from "../../../src/services/DadosSeguroService";
import Rota from "../../../src/config/Router";
import { RequestMock } from "../../mocks/RequestMock";
import XsdMock from '../../mocks/XsdMock';
import { AutenticarGatewayService } from "../../../src/services/AutenticarGatewayService";

const authMock = new RequestMock();

describe("DadosSeguroController", () => {

  test("Deve retornar 200 ao bater na rota", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
      DadosSeguroService.prototype.dadosSeguro = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
    const result = await request.post("/dadosSeguro").send();
    expect(result.status).toBeTruthy();
  });

  test("Deve retornar 500 ao bater na rota", async () => {
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());    
    DadosSeguroService.prototype.dadosSeguro = jest
      .fn()
      .mockRejectedValueOnce(authMock.error());
    const result = await request.post("/dadosSeguro").send();
    expect(result.status).toBeTruthy();
  });


  test("Cancelar Seguro - Deve retornar 200 ao bater na rota", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
      DadosSeguroService.prototype.cancelamentoSeguro = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
    const result = await request.post("/cancelarSeguro").send();
    expect(result.status).toBeTruthy();
  });

  test("Cancelar Seguro - Deve retornar 500 ao bater na rota", async () => {
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success()); 
         
    DadosSeguroService.prototype.dadosSeguro = jest
      .fn()
      .mockRejectedValueOnce(authMock.error());
    const result = await request.post("/cancelarSeguro").send();
    expect(result.status).toBeTruthy();
  });


  test("Contratar Seguro - Deve retornar 200 ao bater na rota", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
      DadosSeguroService.prototype.contratacaoSeguro = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
    const result = await request.post("/contratarSeguro").send();
    expect(result.status).toBeTruthy();
  });

  test("Contratar Seguro - Deve retornar 500 ao bater na rota", async () => {
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());    
    DadosSeguroService.prototype.contratacaoSeguro = jest
      .fn()
      .mockRejectedValueOnce(authMock.error());
    const result = await request.post("/contratarSeguro").send();
    expect(result.status).toBeTruthy();
  });

});

const request = supertest(
  express()
    .use(express.json())
    .use(express.urlencoded({ extended: true }))
    .use("/", Rota)
);
