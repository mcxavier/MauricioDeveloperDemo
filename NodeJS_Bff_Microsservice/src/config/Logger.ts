import express from "express";
import eventLog from "@bradesco/coredig-microsvc-log-js";

const app = express();
eventLog.setExpress(app);

export default class Logger {
  static debugLogs: String[] = [];
  static logs: Map<String, String[]> = new Map<String, String[]>();

  get logger() {
    return this.log;
  }

  public static error(info: String, ...meta: any[]) {
    let msgLog = criarMsg(info, meta);
    new Logger().logger.error(msgLog);
  }

  //public static debug(info: String, ...meta: any[]) {
  //  let msgLog = criarMsg(info, meta);
  //  new Logger().logger.debug(msgLog);
 // }


  public static info(info: String, ...meta: any[]) {
    let msgLog = criarMsg(info, meta);
    new Logger().logger.info(msgLog);
  }

  private log = app.get("logger");

  public static log(req: any, msg?: string) {
    //if (msg) {
      new Logger().logger.info(msg);
    //} 
    //else {
    //  new Logger().logger.info(JSON.stringify(this.logs.get(req.rid)));
   // }
    this.deleteLogs(req);
  }
  
  private static deleteLogs(req: any) {
    this.logs.delete(req.rid);
  }  
}




function criarMsg(msg: String, meta: any[]) {
  let msgLog: String
  if(meta.length > 0) {
    msgLog = msg;
    for (let index = 0; index < meta.length; index++) {
      let element = meta[index];
      if (typeof element != 'string') {
        try {
          element = JSON.stringify(element);
        } catch (err) {
          element = ""
        }
      }
      if(index == 0) {
        msgLog += element;
      } else {
        msgLog += " - " + element;
      }
    }
  } else {
    msgLog = msg;
  }

  return msgLog;
}