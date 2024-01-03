import express from "express";
import { CodigoRetorno } from "../constants/CodigoRetorno";
import { Exception } from "../exception/Exception";

export default class HealthController {

    /**
     * @swagger
     * /health:
     *    get:
     *      summary: Checa a disponibilidade do endpoint.
     *      responses:
     *        200:
     *          description: OK
     *          schema:
     *             type: object
     *             properties:
     *                name:
     *                   type: string
     *                   example: bcpf-cartoes-seguros
     *                status:
     *                   type: string
     *                   example: available
     */
    public check = async (request: express.Request, response: express.Response, next: express.NextFunction) => {

            response.status(CodigoRetorno.OK).json({ name: "bcpf-bff-cartoes-seguros", status: "available" }
            );
    }
}