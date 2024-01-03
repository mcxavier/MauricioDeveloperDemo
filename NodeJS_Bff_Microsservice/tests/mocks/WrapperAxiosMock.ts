import { MockModule } from "./MockModule";
import { AxiosResponse } from "axios";
import { ServerResponse  } from "../../src/utils/http/server-response";
import { CodigoRetorno } from "../../src/constants/CodigoRetorno";
import { responseInject } from "../decorators/index";

const resData = {
    // idSessao: "0003C074B056173X02D2BA81D6AB5F6A",
    // ticket: "DEC8F834D212B637"
    data: "",
    status: 200,
    statusText: "OK"
} as AxiosResponse;

const mock = {
      status: 0
  } as ServerResponse;

export class WrapperAxiosMock implements MockModule< ServerResponse > {

    @responseInject(CodigoRetorno.BAD_REQUEST, "Bad Request", "Bad Request")
    public failure(): ServerResponse {
        return mock;
    }

    @responseInject(CodigoRetorno.OK, "OK", resData)
    public success(): ServerResponse {
        return mock;
    }

    public sucessAxiosResponse(): AxiosResponse {
        return resData;
    }

    @responseInject(CodigoRetorno.SERVICE_UNVAILABLE, "Servico Indisponivel", "Servico Indisponivel")
    public serverError(): ServerResponse {
        return mock;
    }

}
