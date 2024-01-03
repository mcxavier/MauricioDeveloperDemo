import { CodigoRetorno } from "../../src/constants/CodigoRetorno";

export function responseInject(statusCodeResponse: CodigoRetorno = 200, statusMesgResponse: string = "", bodyResponse: any = "") {
    return function(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
        const metodoOriginal = descriptor.value;
        let retornoMetodoOrigem;
        descriptor.value = function(...args: any[]) {
            retornoMetodoOrigem = metodoOriginal.apply(this, args);

            retornoMetodoOrigem = {
                code: statusCodeResponse,
                message: statusMesgResponse,
                details: bodyResponse
            };

            return retornoMetodoOrigem;
        };

        return descriptor;
    };
}
