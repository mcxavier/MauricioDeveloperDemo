using Core.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domains.Customers.Dtos
{
    public class CepDto : IRequest<Response>
    {
        public string numCep { get; set; }
    }
}
