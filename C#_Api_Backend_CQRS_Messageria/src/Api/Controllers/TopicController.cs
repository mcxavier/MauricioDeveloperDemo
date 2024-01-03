using Infra.QueryCommands.Commands.Topics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.SharedKernel;
using Api.App_Infra;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/topic"), ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    public class TopicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost, Route("location")]
        public async Task<IActionResult> PublishLocation([FromBody] PublishApplicationLocation location)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(location);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de localização");
                }
                return Ok(response);
            }
            catch
            {

                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar localização";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }

                return StatusCode(errorCode, errorResponse);
            }
        }

        [HttpPost, Route("customer")]
        public async Task<IActionResult> PublishCustomer([FromBody] PublishApplicationCustomer customer)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(customer);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de cliente");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar Cliente";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }

                return StatusCode(errorCode, errorResponse);
            }

        }

        [HttpPost, Route("price")]
        public async Task<IActionResult> PublishPrice([FromBody] PublishApplicationPrice price)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(price);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de preço");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar preço";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }
                return StatusCode(errorCode, errorResponse);
            }
        }

        [HttpPost, Route("product")]
        public async Task<IActionResult> PublishProduct([FromBody] PublishApplicationProduct product)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(product);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de produto");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar produto";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }
                return StatusCode(errorCode, errorResponse);
            }
        }

        [HttpPost, Route("stock")]
        public async Task<IActionResult> PublishStock([FromBody] PublishApplicationStock stock)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(stock);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de estoque");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar estoque";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }
                return StatusCode(errorCode, errorResponse);
            }
        }

        [HttpPost, Route("salesAgent")]
        public async Task<IActionResult> PublishSellers([FromBody] PublishApplicationSellers seller)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(seller);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de vendedores");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar vendedores";
                if (response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }
                return StatusCode(errorCode, errorResponse);
            }
        }


        [HttpPost, Route("order")]
        public async Task<IActionResult> PublishOrder([FromBody] PublishApplicationOrder order)
        {
            Response response = null;
            try
            {
                response = await _mediator.Send(order);
                if (response.IsError)
                {
                    throw new Exception("Erro no cadastro de venda");
                }
                return Ok(response);
            }
            catch
            {
                int errorCode = 500;
                string errorResponse = "Erro ao cadastrar venda";
                if (response.Errors != null && response.Errors.Any())
                {
                    errorCode = response.Errors.FirstOrDefault().Code;
                    errorResponse = response.Errors.FirstOrDefault().Message;
                }
                return StatusCode(errorCode, errorResponse);
            }
        }
    }
}
