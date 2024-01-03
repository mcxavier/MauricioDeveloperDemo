# Seguro Super Protegido
# BCPF-BFF-CARTOES-SEGUROS

Este serviço trata-se de um backend for frontend para contratação do Seguro Super Protegido de cartões.

<hr>

## Requisitos
* NodeJS v10.15.0
* TypeScript v3.4.2+ | Target ES6
* Swagger
* Jest
* Express

## Referenciar repositórios Nexus

1. O aquivo '.npmrc' deve incluir as configurações:
```
registry = https://nexusrepository.bradesco.com.br:8443/repository/mobilepf-npm-central
strict-ssl = false
_auth = Base64("{Chave M}:{Senha}")
```

## Deploy do Projeto

**Depois de clonar o projeto e entrar no diretório em um terminal:**
```bash
# instalar dependências
npm install

# gerar documentação - swagger
npm run swagger-generate

# construir
npm run build

# executar
npm run start:dev
```
<hr>

# Documentação de API - Swagger

Instalar o [tsoa](https://www.npmjs.com/package/tsoa)
```bash
npm i tsoa --save-dev
```
Instalar o [Swagger UI Express](https://www.npmjs.com/package/swagger-ui-express):
```bash
npm i swagger-ui-express  --save-dev
```