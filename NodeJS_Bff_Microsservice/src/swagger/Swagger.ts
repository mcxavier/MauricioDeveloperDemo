import swaggerJsdoc from "swagger-jsdoc";

const options = {
  swaggerDefinition: {
    openapi: "3.0.1",
    info: {
      description: "API responsável por tratamentos backend para o Menu de Cartões",
      title: "Backend For Frontend - Menu Cartões",
      version: "1.0.0",
      contact: {
        name: "Bradesco",
        email: "contato@bradesco.com.br",
        url: "https://www.bradesco.com.br/"
      },
      license: {
        name: "Apache 2.0",
        url: "https://www.apache.org/licenses/LICENSE-2.0.html"
      }
    },
    basePath: "./",
    host: "localhost:8080",
    produces: ["application/json"],
    servers: [
      {
        url: "http://localhost:8080",
        description: "LOCAL server"
      }
    ],
  },
  apis: [
    "./src/controllers/**/*Controller.{ts,js}"
  ],

};

export default swaggerJsdoc(options);
