import router from "../../../src/config/Router";
import { express as stateless } from '@bradesco/coreopen-stateless-js';
import bodyParser from "body-parser";
import express from "express";
import supertest from "supertest";
import XSD from "../../mocks/XsdMock";
import Rota from "../../../src/config/Router";
import { RequestMock } from "../../mocks/RequestMock";
import XsdMock from '../../mocks/XsdMock';
import { ExceptionUtils } from "../../../src/exception/ExceptionUtils";
import { ServerResponse } from 'http';

test("should test routes", () => {
    expect(router).toBeTruthy();
});




describe("Router", () => {
    test("health", async () => {
      const result = await request.get("/health").send();
      expect(result.status).toBeTruthy();
    });
  
    test("listarCartoes", async () => {
      const result = await request.get("/listarCartoes").send();
      expect(result.status).toBeTruthy();
    });

    test("dadosSeguro", async () => {
        const result = await request.get("/dadosSeguro").send();
        expect(result.status).toBeTruthy();
    });
  
    test("contratarSeguro", async () => {
        const result = await request.post("/contratarSeguro").send();
        expect(result.status).toBeTruthy();
    });
  
    test("cancelarSeguro", async () => {
        const result = await request.post("/cancelarSeguro").send();
        expect(result.status).toBeTruthy();
    });
  
    test("listarBeneficiosSeguro", async () => {
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
  
  