import { CodigoRetorno } from "../../src/constants/CodigoRetorno";
import { ServerResponse } from "../../src/utils/http/server-response";
import { MockModule } from "./MockModule";

const resData = { it: "work's" };

export class RequestMock implements MockModule< ServerResponse > {

    public failure(): ServerResponse {
        return {status: CodigoRetorno.NOT_FOUND, body: "Not Found", headers: "Not Found"};
    }

    public error(): ServerResponse {
        return {status: CodigoRetorno.INTERNAL_SERVER_ERROR, body: "Not Found", headers: "Not Found"};
    }

    public success(): ServerResponse {
        return {status: CodigoRetorno.OK, body: "OK", headers: resData};
    }

    public successNoContent(): ServerResponse {
        return {status: CodigoRetorno.NO_CONTENT, body: "OK", headers: resData};
    }

    public indisponibilidade(): ServerResponse {
        return {status: CodigoRetorno.SERVICE_UNVAILABLE, body: "Servico Indisponivel", headers: resData};
    }

    public pedidoRuim(): ServerResponse {
        return {status: CodigoRetorno.BAD_REQUEST, body: "Pedido ruim", headers: resData};
    }
}