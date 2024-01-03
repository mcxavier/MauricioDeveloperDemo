import Logger from "../../../src/config/Logger";
import { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import express from 'express';

jest.setTimeout(700);
const req = {headers: {}} as express.Request;

describe("Teste de criacao do Logger", () => {
  test("Verifica a instancia de Logger", async () => {
    const log = new Logger();
    expect(log).toBeInstanceOf(Logger);
  });

  test("Verifica o metodo error", () => {
    jest.spyOn(Logger, "error");
    Logger.error("Teste", req);
    expect(Logger.error).toBeCalled();
  });

  test("Verifica o metodo info", () => {
    jest.spyOn(Logger, "info");
    Logger.info("Teste", req);
    expect(Logger.info).toBeCalled();
  });

  test("Verifica a metodo info com erro no JSON.stringify", () => {
    jest.spyOn(Logger, "info");
    jest.spyOn(JSON, "stringify").mockImplementationOnce(() => {
      throw new Error();
    })
    var info = {
      "message": "mensagem erro"
    }
    Logger.info("Erro", req, info);
    expect(Logger.info).toBeCalled();
  });

  test("Verifica a metodo info com 3 parâmetros", () => {
    jest.spyOn(Logger, "error");
    var info = {
      "message": "mensagem erro"
    }
    Logger.error("Erro", req, info, "erro2");
    expect(Logger.error).toBeCalled();
  });

  test("Verifica método log com msg", () => {
    jest.spyOn(Logger, "log");
    Logger.log(req, "Teste");
    expect(Logger.log).toBeCalled();
  })
});