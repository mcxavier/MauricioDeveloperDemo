import { MCENTHelper } from '@bradesco/bcpf-lib-mcent/lib/helper';
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import { Request, Response } from "express";
import XSD from "../../mocks/XsdMock";
import { Mcent } from "./../../../src/utils/Mcent"
const reqDadosInferencia = require('@bradesco/bcpf-lib-mcent/lib/model/dados-inferencia')

declare module "express-serve-static-core" {
  interface Response {
    error: (code: number, message: string) => Response;
    success: (code: number, message: string, result: any) => Response;
  }
}

describe("Mcent", () => {
  let mcent: Mcent;
  let res: Response;

  const req = {
    body: {
      passo: 2,
    },
    headers: {}
  } as Request;

  const req2 = {
    body: {
      passo: 2,
    }
  } as Request;

  test("Deve instanciar o mcent", () => {
    mcent = new Mcent();
    expect(mcent).toBeTruthy();
  });

  test("Deve gravarMcent", async () => {
    mcent = new Mcent();
    const logDados = jest.spyOn(mcent, "logarDadosInferencia");

    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    req.headers["dispositivoseguranca"] = "{\"tipo\": \"1\", \"numeroSerie\": \"123\"}";
    req.headers["host"] = "{\"tipo\": \"1\"}";

    await mcent.gravarMcent(req, res, "1", "5");

    expect(logDados).toBeTruthy();
  });

  test("Deve gravarMcent", async () => {
    mcent = new Mcent();
    const logDados = jest.spyOn(mcent, "logDadosLibAntiFraude");

    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    req.headers["dispositivoSeguranca"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-for"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-port"] = "{\"tipo\": \"1\"}";

    await mcent.gravarMcent(req, res, "1", "5");

    expect(logDados).toBeTruthy();
  });

  test("Deve testar getStatelessHeadersBase64 com sucesso", () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    const xStatelessOpen = "ewogICJwZXJpZmVyaWNvIjogIjQ1NjA2MzczMDU0NDEwMSIsCiAgImlkaW9tYSI6IDEsCiAgImNhbmFsIjogNjYsCiAgImVtcHJlc2EiOiAyMzcsCiAgInV1aWQiOiAiYjdjMzU2ZmItMjQ2OS00NzI1LWFmZjEtZmE4NmVmZDljMDU4IiwKICAiZGVwZW5kZW5jaWEiOiAxLAogICJjbGllbnRlIjogewogICAgImRhYyI6ICI3IiwKICAgICJ0aXBvQ2xpZW50ZSI6ICJGIiwKICAgICJjcGZDbnBqIjogIjcyNTIyMzkwMzAwMDA5MiIsCiAgICAicGowMCI6IGZhbHNlLAogICAgImFnZW5jaWEiOiAiMzk5NSIsCiAgICAicmF6YW8iOiAiMDcwNSIsCiAgICAidGl0dWxhcmlkYWRlIjogIjEiLAogICAgImNvbnRhIjogIjI3MTk0OCIsCiAgICAibm9tZUNsaWVudGUiOiAiSlVMSUFOQSBTSUxWRVRSRSBESUFTIiwKICAgICJ0aXBvQ29udGEiOiAiRkFDSUwiCiAgfSwKICAiYWxnIjogIkVTMjU2Igp9";
    const xStatelessClosed = "eyJmcndrIjp7InRpY2tldCI6IjZCRjk0QTJEMTA5RDgxQjUiLCJpZHNlc3NhbyI6IjAwMDNDMDc0QjA1NjFEMVgwMkNDM0EzOTBCMkYzRTdFIiwidXN1YXJpbyI6IjAzODY0MDAwMDAwMDE5MDcwNTAxIiwidGlwb1VzdWFyaW8iOiJDT05UQSJ9fQ";

    const suit = mcent.getStatelessHeadersBase64(req, res);

    expect(suit["x-stateless-open"]).toEqual(xStatelessOpen);
    expect(suit["x-stateless-closed"]).toEqual(xStatelessClosed);
  });

  test("Deve gerarErro ao logDadosLibAntiFraude", async () => {
    mcent = new Mcent();
    const logDados = jest.spyOn(mcent, "logDadosLibAntiFraude");

    await mcent.gravarMcent(req, res, "1", "5");

    expect(logDados).toBeTruthy();
  });

  test("Deve gerar sucesso ao logarDadosInferencia", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    jest.spyOn(MCENTHelper.prototype, "gravarDadosMcentEvento").mockImplementation(() => {
      return Promise.resolve("Success");
    })

    const gravarDadosMcentEvento = jest.spyOn(MCENTHelper.prototype, "gravarDadosMcentEvento");

    const dadosInferencia = new reqDadosInferencia.DadosInferencia({ dataCriacaoRegistro:"", scrambled:"", ipOrigem:"", numPasso:0, dadosTransacao:"", valorTransacao:0, userAgent:"", numSerieDisp:"", tipoDisp:"", codGrupoFuncionalidade:0, codFuncionalidade:0, bancoOrigem:237, agenciaOrigem:"", contaOrigem:"", indicatTitOrigem:"", portaOrigem:"", });
    
    req.headers["dispositivoSeguranca"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-for"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-port"] = "{\"tipo\": \"1\"}";

   const suit = mcent.logarDadosInferencia(dadosInferencia, req.headers)

   expect(gravarDadosMcentEvento).toBeCalledTimes(1);
  });

  test("Deve gerar exception ao logarDadosInferencia", async () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    jest.spyOn(MCENTHelper.prototype, "gravarDadosMcentEvento").mockImplementation(() => {
       throw new Error("Teste Erro");
    })

    const dadosInferencia = new reqDadosInferencia.DadosInferencia({ dataCriacaoRegistro:"", scrambled:"", ipOrigem:"", numPasso:0, dadosTransacao:"", valorTransacao:0, userAgent:"", numSerieDisp:"", tipoDisp:"", codGrupoFuncionalidade:0, codFuncionalidade:0, bancoOrigem:237, agenciaOrigem:"", contaOrigem:"", indicatTitOrigem:"", portaOrigem:"", });
    
    req.headers["dispositivoSeguranca"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-for"] = "{\"tipo\": \"1\"}";
    req.headers["x-forwarded-port"] = "{\"tipo\": \"1\"}";

    const suit = mcent.logarDadosInferencia(dadosInferencia, req.headers);
    expect(suit).toBeTruthy();
  });

  test("Deve testar getDadosInferencia com sucesso", () => {
    jest.spyOn(stateless, "getXSD").mockImplementation(() => {
      return XSD;
    });

    const suit = mcent.getDadosInferencia(req, res, 1);

    expect(suit).toBeTruthy();
  });
});