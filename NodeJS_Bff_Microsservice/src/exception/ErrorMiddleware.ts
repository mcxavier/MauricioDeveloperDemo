import { NextFunction, Request, Response } from "express";
import { Exception } from "./Exception";

function errorMiddleware(error: Exception, request: Request, response: Response, next?: NextFunction) {
   
}

export default errorMiddleware;
