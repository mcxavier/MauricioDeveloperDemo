import { ServerResponse } from './server-response';

export interface HttpClient {
  get(url: string, options?: object): Promise<ServerResponse>;

  post(url: string, body: object, options?: object): Promise<ServerResponse>;
}