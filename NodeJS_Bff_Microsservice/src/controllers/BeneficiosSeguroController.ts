import { BeneficiosSeguroService } from "../services/BeneficiosSeguroService";
import Logger from "../config/Logger";
import { express as stateless } from "@bradesco/coreopen-stateless-js";
import express from "express";
import { Exception } from "../exception/Exception";

export default class BeneficiosSeguroController {
    /**
     * @swagger
     * /health:
     *    get:
     *      summary: Retornar dados do seguro.
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

    constructor(
        private service: BeneficiosSeguroService = new BeneficiosSeguroService()
    ) { }

    public listaBeneficiosSeguro = async (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {
            Logger.info("BeneficiosSeguroBusiness.beneficiosSeguro.init"); 

            const beneficios = await this.service.beneficiosSeguro(request, response).catch((error) => error);

            Logger.info("Retorno Service - BeneficiosSeguroBusiness.beneficiosSeguro: ", beneficios);

            response.status(beneficios.status).json(beneficios.body);
            


            // response.status(200).json({
            //     titulo:"Mais proteção para seu cartão em caso de perda, roubo ou ameaça",
            //     subtitulo:"Por R$ 9,99 ao mês, o Superprotegido te deixa ainda mais seguro e livre de preocupações.",
            //     beneficios:[
            //           {texto: "Proteção de até R$ 50 mil para compras e aluguel de bens, inclusive pela internet",
            //            imagem: "seguro-protecao-preco.svg"},
            //           {texto: "Proteção de até R$ 5 mil para saques sob ameaça com o cartão de crédito",
            //            imagem: "atm-saque.svg"},
            //           {texto: "Em caso de morte, os gastos da fatura a vencer serão quitados até R$ 50 mil",
            //            imagem: "category-heart.svg"},
            //           {texto:"Assistências como chaveiro, proteção residencial (roubo e furto), entre outros",
            //            imagem: "category-tools.svg"},
            //           {texto:"Concorra a R$12 mil semanais",
            //           imagem: "financial-dolar.svg"},
            //     ],
            // });

            
        } catch (err) {
            next(new Exception(err.status, err.body));
        }
    }


/*     public listaBeneficiosSeguroMock = (request: express.Request, response: express.Response, next: express.NextFunction) => {
        try {

            response.status(200).json("{texto: mauricao é do caralho}");
            
        } catch (err) {
            next(new Exception(err.status, err.body));
        }
    }
 */
}