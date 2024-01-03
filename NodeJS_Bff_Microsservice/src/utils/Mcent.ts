import { MCENTHelper } from "@bradesco/bcpf-lib-mcent/lib/helper/index";
import { DadosInferencia } from "@bradesco/bcpf-lib-mcent/lib/model/dados-inferencia";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import express, { response } from "express";
import Logger from "../config/Logger";
const reqDadosInferencia = require('@bradesco/bcpf-lib-mcent/lib/model/dados-inferencia')

export class Mcent {
  constructor() {}

  logger = new Logger().logger;
  cGrpFunclIb = 244
  cFunclIb = 5
  GRAVACAOPRETRANSACAO = 1; //DEFINE QUE ESTA GERANDO LOG ANTES DA TRANSAÇÃO FINAL
  GRAVACAOAPOSTRANSACAO = 2; //DEFINE QUE ESTA GERANDO LOG DEPOIS DA TRANSAÇÃO FINAL
  FUNCIONALIDADE;
  GRUPOFUNCIONALIDADE;

  async gravarMcent(req: express.Request, res: express.Response, passo: any, funcionalidade: any, grupo: any = 244) {
    Logger.info("gravarMcent.init");
    Logger.info("gravarMcent >>>>>> PASSO", passo);
    this.FUNCIONALIDADE = funcionalidade;
    this.GRUPOFUNCIONALIDADE = grupo;
    await this.logDadosLibAntiFraude(req, res, passo);
  }

  async logDadosLibAntiFraude(req: express.Request, res: express.Response, passo: number) {
    try {
      Logger.info("logDadosLibAntiFraude.init");
      // setLog.LOGGER.info("logDadosLibAntiFraude.init new setLogger");
      const dadosInferencia = this.getDadosInferencia(req, res, passo);
      await this.logarDadosInferencia(dadosInferencia, this.getStatelessHeadersBase64(req, res));
    } catch (error) {
      Logger.info(error.message);
      Logger.info(error);
    }
  }

  getDadosInferencia(req, res, passo) {

    Logger.info(">>>>>>>>>>>>> PASSO: ", passo);
    Logger.info(">>>>>>>>>>>>> REQ: ", req);
    Logger.info(">>>>>>>>>>>>> RES: ", res);

    var dadosInferencia = new reqDadosInferencia.DadosInferencia({ dataCriacaoRegistro:Date, scrambled:"", ipOrigem:"", numPasso:0, dadosTransacao:"", valorTransacao:0, userAgent:"", numSerieDisp:"", tipoDisp:"", codGrupoFuncionalidade:0, codFuncionalidade:0, bancoOrigem:237, agenciaOrigem:"", contaOrigem:"", indicatTitOrigem:"", portaOrigem:"", });
  
      const xsd = stateless.getXSD(res);
      Logger.info(">>>>>>>>>>>>> XSD: ", xsd);

      Logger.info(">>>>>>>>>>>>> DISPOSITIVO SEGURANCA: ", req.headers.dispositivoseguranca);

      const dispSeg = JSON.parse(JSON.stringify(req.headers.dispositivoseguranca));
      Logger.info(">>>>>>>>>>>>> DISPOSITIVO SEGURANCA: ", dispSeg);

      dadosInferencia.scrambled = xsd.open.uuid;
      dadosInferencia.agenciaOrigem = xsd.open.cliente.agencia;
      dadosInferencia.contaOrigem = xsd.open.cliente.conta;
      dadosInferencia.indicatTitOrigem = xsd.open.cliente.titularidade;
      dadosInferencia.numPasso = passo;  
  
      if(dispSeg != "" &&  dispSeg != undefined){
        if (dispSeg.referencia == "" || dispSeg.referencia == undefined ) {
          dadosInferencia.numSerieDisp = "0";
        } else {
          dadosInferencia.numSerieDisp = dispSeg.referencia;
        }

        if (dispSeg.tipo == "" || dispSeg.tipo == undefined ) {
          dadosInferencia.tipoDisp = "0";
        } else {
          dadosInferencia.tipoDisp = dispSeg.tipo;
        }
      }else{
        dadosInferencia.tipoDisp = "0";
        dadosInferencia.numSerieDisp = "0";
      }
    
      dadosInferencia.codGrupoFuncionalidade = this.GRUPOFUNCIONALIDADE;
      dadosInferencia.codFuncionalidade = this.FUNCIONALIDADE;
      dadosInferencia.userAgent =  req.body.userAgent;
      dadosInferencia.dadosTransacao = req.body.dadosTransacao;
      dadosInferencia.dadosTransacaoFinanc = req.body.dadosTransacao;
      dadosInferencia.valorTransacao = 0.00;
      dadosInferencia.ipOrigem = req.headers['x-forwarded-for'] == undefined ? req.headers['host'].substring(0,req.headers['host'].indexOf(":")) : req.headers['x-forwarded-for'];
      dadosInferencia.portaOrigem = req.headers['x-forwarded-port'] == undefined ? req.headers['host'].substring(req.headers['host'].indexOf(":") +1, req.headers['host'].size) : req.headers['x-forwarded-port'];
      dadosInferencia.dataCriacaoRegistro = new Date();
    
      Logger.info(">>>>>>>>>>>>>>>  dados inferencia ", dadosInferencia, " <<<<<<<<<<<<<< dados inferencia");
  
    return dadosInferencia;
  
  }

  getStatelessHeadersBase64(req: express.Request, res: express.Response) {
    Logger.info("Recuperando Headers descriptografados, e colocando em base64 para enviar ao SRV");
    const xsd = stateless.getXSD(res);
    const headers = {};
    xsd.exportTo(headers);
    return headers;
  }

  async logarDadosInferencia(dadosInferencia: DadosInferencia, headers: any) {
    try {
      Logger.info("logarDadosInferencia.init");
      const libKafkaAntiFraude = new MCENTHelper(this.logger);
      const status = await libKafkaAntiFraude.gravarDadosMcentEvento(dadosInferencia, headers);
    } catch (error) {
      Logger.info("Gravar mcent error: ", error);
      throw error;
    }
  }


}