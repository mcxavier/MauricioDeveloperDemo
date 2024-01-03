#Auto generated Octane revision tag
@TID71221REV0.8.0
Feature: Funcionalidade para verificar health

   Scenario Outline: Verifica se é possível chamar API Health usando método GET
    Given eu quero chamar no "<service>" o seguinte "<endpoint>"
    When  envio a solicitação GET
    Then eu valido o codigo de status <statuscode>
    
    Examples: 
      | service          		  	    | endpoint 				| statuscode |
      | bcpf-bff-cartoes-seguros  	| health  	        | 200        | 