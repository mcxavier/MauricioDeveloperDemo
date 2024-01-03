import Logger from "../config/Logger";
import express, { express as stateless } from "@bradesco/coreopen-stateless-js";

export function getXsd(response: express.Response, request: express.Request) {
    const xsd = stateless.getXSD(response);
    return xsd;
  }