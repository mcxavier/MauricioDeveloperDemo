import { Exception } from "./Exception";
import { CodigoRetorno } from "../constants/CodigoRetorno";
import { MensagemRetorno } from "../constants/MensagenRetorno";
import { ExceptionErroSistema } from "./ExceptionErroSistema";

export class ExceptionUtils {

    //public static indisponibilidadeErroSistema(msg) {
    //    return new Exception(CodigoRetorno.SERVICE_UNVAILABLE, msg);
   // }

    public static indisponibilidade() {
        return new Exception(CodigoRetorno.SERVICE_UNVAILABLE, MensagemRetorno[CodigoRetorno.SERVICE_UNVAILABLE]);
    }

    public static proibicao() {
        return new Exception(CodigoRetorno.FORBIDDEN, MensagemRetorno[CodigoRetorno.FORBIDDEN]);
    }

    public static pedidoRuim() {
        return new Exception(CodigoRetorno.BAD_REQUEST, MensagemRetorno[CodigoRetorno.BAD_REQUEST]);
    }
}
