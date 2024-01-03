import { ExceptionUtils } from "../../../src/exception/ExceptionUtils";
import { CodigoRetorno } from "../../../src/constants/CodigoRetorno";
import { MensagemRetorno } from "../../../src/constants/MensagenRetorno";
import { Exception } from "../../../src/exception/Exception";


describe("ExceptionUtils", () => {

    test("should test indisponibilidade", () => {
        const error: Exception = ExceptionUtils.indisponibilidade();
        expect(error).toBeTruthy();
        expect(error.message).toBe(MensagemRetorno[CodigoRetorno.SERVICE_UNVAILABLE]);
        expect(error.status).toBe(CodigoRetorno.SERVICE_UNVAILABLE);
    });

    test("should test proibicao", () => {
        const error: Exception = ExceptionUtils.proibicao();
        expect(error).toBeTruthy();
        expect(error.message).toBe(MensagemRetorno[CodigoRetorno.FORBIDDEN]);
        expect(error.status).toBe(CodigoRetorno.FORBIDDEN);
    });

    test("should test pedidoRuim", () => {
        const error: Exception = ExceptionUtils.pedidoRuim();
        expect(error).toBeTruthy();
        expect(error.message).toBe(MensagemRetorno[CodigoRetorno.BAD_REQUEST]);
        expect(error.status).toBe(CodigoRetorno.BAD_REQUEST);
    });

});