import  errorMiddleware from "../../../src/exception/ErrorMiddleware";
import { Exception } from "../../../src/exception/Exception";
import express from "express";


const req = {} as express.Request;
const res = express().response;
res.status = () => res;
res.send = () => res;
test("should test ErrorMiddleware", () => {
    const err = new Exception(500, "Error");
    expect(errorMiddleware(err, req, res)).toBe(undefined);
});