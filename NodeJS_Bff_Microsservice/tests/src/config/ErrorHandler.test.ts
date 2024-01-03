import { Exception } from "../../../src/exception/Exception";

test("should test ErrorHandler", () => {
    const errorHandler = new Exception(500, "");
    expect(errorHandler).toBeTruthy();
});