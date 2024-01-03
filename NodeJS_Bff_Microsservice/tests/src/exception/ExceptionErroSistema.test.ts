import { ExceptionErroSistema} from "../../../src/exception/ExceptionErroSistema";
import { Exception } from "../../../src/exception/Exception";

describe("ExceptionErroSistema", () => {

    test("should test ErrorHandler", () => {
        const errorHandler = new ExceptionErroSistema(500, "");
        expect(errorHandler).toBeTruthy();
    });
    
});