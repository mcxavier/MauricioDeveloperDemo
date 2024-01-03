import { express as stateless } from '@bradesco/coreopen-stateless-js';
import bodyParser from "body-parser";
import express from "express";
import supertest from "supertest";
import XSD from "../../mocks/XsdMock";
import { BeneficiosSeguroService } from "../../../src/services/BeneficiosSeguroService"; 
import Rota from "../../../src/config/Router";
import { RequestMock } from "../../mocks/RequestMock";
import XsdMock from '../../mocks/XsdMock';
import { ExceptionUtils } from "../../../src/exception/ExceptionUtils";
import { AutenticarGatewayService } from "../../../src/services/AutenticarGatewayService";
import { ServerResponse } from 'http';


const authMock = new RequestMock();

describe("BeneficiosSeguroController", () => {
  test("Deve retornar 200 ao bater na rota", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });
    AutenticarGatewayService.prototype.autenticarGatewayCartoes = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
      BeneficiosSeguroService.prototype.beneficiosSeguro = jest
      .fn()
      .mockResolvedValueOnce(authMock.success());
    const result = await request.get("/listarBeneficiosSeguro").send();
    expect(result.status).toBeTruthy();
  });

  test("Deve retornar 500 ao bater na rota", async () => {
    BeneficiosSeguroService.prototype.beneficiosSeguro = jest
      .fn()
      .mockRejectedValueOnce(authMock.error());
    const result = await request.get("/listarBeneficiosSeguro").send();
    expect(result.status).toBeTruthy();
  });
});

const request = supertest(
  express()
    .use(express.json())
    .use(express.urlencoded({ extended: true }))
    .use("/", Rota)
);
