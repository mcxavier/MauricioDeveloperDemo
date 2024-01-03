
import express from "express";
import supertest from "supertest";
import Rota from "../../../src/config/Router";

describe("Health Controller", () => {

  test("Health GET", async () => {
    const result = await request
      .get("/health")
      .send();

    expect(result.status).toBeTruthy();
    //expect(result.type).toEqual("application/json");
  });
});

const request =
  supertest(
    express()
      .use(express.json())
      .use(express.urlencoded({ extended: true }))
      .use("/", Rota)
  );