import axios from "axios";
import WrapperAxios from "../../../src/infra/WrapperAxios";
import { WrapperAxiosMock } from "../../mocks/WrapperAxiosMock";

const AxiosMock = new WrapperAxiosMock();

const mock = jest.fn();

jest.mock("axios");

const urlMock = "http://www.urlMock.com";

const obj = {
  cpf: "123.123.134",
  codParceiro: 21,
  nomeParceiro: "Teste",
  codSegmento: 100,
  nomeSegmento: "Segmento"
};

describe("WrapperAxios", () => {

    test("Deve retornar status 200 ao acionar metodo: GET, no WrapperAxios", async () => {
      axios.get = mock;
      mock.mockResolvedValue(AxiosMock.sucessAxiosResponse());

      const headers = {
        "Content-Type": "application/json"
      };

      const result = await new WrapperAxios().get(urlMock, headers);
      expect(result.status).toBe(200);

    });

    test("Deve retornar status 500 ao acionar metodo: GET, no WrapperAxios", async () => {
      axios.get = mock;
      mock.mockImplementationOnce(() => Promise.reject({response: {
        status: 500,
        statusText: "OK",
        body: {
          idSessao: "0003C074B056173X02D2BA81D6AB5F6A",
          ticket: "DEC8F834D212B637"
        }
      }}));

      const headers = {
        "Content-Type": "application/json"
      };
      try {
        await new WrapperAxios().get(urlMock, headers);
      } catch (error) {
        expect(error.code).toBe(undefined);
      }

    });

    test("Deve retornar status 200 ao acionar metodo: POST, no WrapperAxios", async () => {
      axios.post = mock;
      mock.mockResolvedValue(AxiosMock.sucessAxiosResponse());

      const headers = {
        "Content-Type": "application/json"
      };

      const result = await new WrapperAxios().post(urlMock, obj, headers);

      expect(result.status).toBe(200);
    });

    test("Deve retornar status 500 ao acionar metodo: POST, no WrapperAxios", async () => {

        axios.post = mock;
        mock.mockImplementationOnce(() => Promise.reject({response: {
          status: 500,
          statusText: "Erro",
          body: {
            idSessao: "0003C074B056173X02D2BA81D6AB5F6A",
            ticket: "DEC8F834D212B637"
          }
        }}));

        const headers = {
          "Content-Type": "application/json"
        };
        try {
          await new WrapperAxios().post(urlMock, obj, headers);
        } catch (error) {
          expect(error.code).toBe(undefined);
        }

    });

    test("Deve retornar status 200 ao acionar metodo: PUT, no WrapperAxios", async () => {
      axios.put = mock;
      mock.mockResolvedValue(AxiosMock.sucessAxiosResponse());

      const headers = {
        "Content-Type": "application/json"
      };

      const result = await new WrapperAxios().put(urlMock, obj, headers);

      expect(result.status).toBe(200);
    });

    test("Deve retornar status 500 ao acionar metodo: PUT, no WrapperAxios", async () => {

        axios.put = mock;
        mock.mockImplementationOnce(() => Promise.reject({response: {
          status: 500,
          statusText: "Error",
          body: {
            idSessao: "0003C074B056173X02D2BA81D6AB5F6A",
            ticket: "DEC8F834D212B637"
          }
        }}));

        const headers = {
          "Content-Type": "application/json"
        };
        try {
          await new WrapperAxios().put(urlMock, obj, headers);
        } catch (error) {
          expect(error.code).toBe(undefined);
        }

    });

    test("Deve retornar status 200 ao acionar metodo: DELETE, no WrapperAxios", async () => {
      axios.delete = mock;
      mock.mockResolvedValue(AxiosMock.sucessAxiosResponse());

      const headers = {
        "Content-Type": "application/json"
      };

      const result = await new WrapperAxios().delete(urlMock, obj, headers);

      expect(result.status).toBe(200);
    });

    test("Deve retornar status 500 ao acionar metodo: DELETE, no WrapperAxios", async () => {

        axios.delete = mock;
        mock.mockImplementationOnce(() => Promise.reject({response: {
          statusText: "Error",
          body: {
            idSessao: "0003C074B056173X02D2BA81D6AB5F6A",
            ticket: "DEC8F834D212B637"
          }
        }}));

        const headers = {
          "Content-Type": "application/json"
        };
        try {
          await new WrapperAxios().delete(urlMock, obj, headers);
        } catch (error) {
          expect(error.code).toBe(undefined);
        }
    });
});
