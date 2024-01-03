import { ServerResponse } from "../utils/http/server-response";

export default interface WrapperHttpClient {
    get(url: string, config?: object): Promise<ServerResponse>;
    post(url: string, data?: object, config?: object): Promise<ServerResponse>;
    put(url: string, data?: object, config?: object): Promise<ServerResponse>;
    delete(url: string, data?: object, config?: object): Promise<ServerResponse>;
}
