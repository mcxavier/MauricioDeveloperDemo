const stateless = require("@bradesco/coreopen-stateless-js").express;
import express from "express";
import router from "./config/Router";
import errorMiddleware from "./exception/ErrorMiddleware";
import tracing from "@bradesco/coredig-microsvc-headers";
import Logger from "./config/Logger";
import ruid from 'express-ruid';
import { env } from './env';

const eventLog = require('@bradesco/coredig-microsvc-log-js');

class App {
  public app;

  constructor() {
    this.app = express();
    this.config();
  }

   private async config() {

    this.app.use(ruid());

    this.app.set("case sensitive routing", true);
    this.app.use(tracing());
    this.app.use(express.json());
    this.app.use(express.urlencoded({ extended: true }));
    /**
     * Stateless Config
     */
    this.app.use(stateless.importFromRequest);
    this.app.use("/", router);
    this.app.use(errorMiddleware);

    const port = 8080;
    this.app.set("port", port);

    this.app
        .listen(port, () => {
            const startupMsg = `bcpf-bff-cartoes-seguros running on port ${port} as ${process.env.NODE_ENV} mode...`;
            Logger.log("", startupMsg);
        });
}

}

export default new App().app;