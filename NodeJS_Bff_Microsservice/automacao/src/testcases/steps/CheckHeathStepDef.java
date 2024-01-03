package Steps;


import java.util.Properties;

import org.junit.Assert;

import cucumber.api.PendingException;
import cucumber.api.java.en.Given;
import cucumber.api.java.en.Then;
import cucumber.api.java.en.When;
import io.restassured.RestAssured;
import io.restassured.response.Response;
import io.restassured.specification.RequestSpecification;

public class CheckHeathStepDef {
	
	RequestSpecification request;
	Response response = null;
	String SERVICE_BaseUrl;
	String environment;
	String service;
	
	@Given("^eu quero chamar no \"([^\"]*)\" o seguinte \"([^\"]*)\"$")
	public void eu_quero_chamar_no_o_seguinte(String servico, String endpoint) throws Throwable {	

		environment = utilitarios.Funcoes.ReturnEnvironment();
		Properties prop = utilitarios.Funcoes.getprop();
		SERVICE_BaseUrl = prop.getProperty("prop.server.host." + environment);
		service = servico;
	}

	@When("^envio a solicitação GET$")
	public void envio_a_solicitação_GET() throws Throwable {
		request = RestAssured.given();
		response = request.when().relaxedHTTPSValidation().get("https://" + SERVICE_BaseUrl + "/health");
		
	}
	
	@Then("^eu valido o codigo de status (\\d+)$")
	public void eu_valido_o_codigo_de_status(int statuscode) throws Throwable {
		Assert.assertEquals(statuscode, response.getStatusCode());

	}


}
