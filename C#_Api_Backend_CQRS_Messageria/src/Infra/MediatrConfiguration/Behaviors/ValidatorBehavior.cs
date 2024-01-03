using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Infra.QueryCommands._Kernel.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Utils.Extensions;

namespace Infra.QueryCommands._Kernel.Behaviors
{

    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            this._validators = validators ?? throw new ArgumentException(nameof(IValidator<TRequest>));
            this._logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            this._logger.LogInformation("Validating command {CommandType}", typeName);
            var failures = this._validators
                               .Select(validator => validator.Validate(request))
                               .SelectMany(result => result.Errors)
                               .Where(error => error != null)
                               .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

                throw new CommandValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures.Select(x => x.ErrorMessage).ToArray());
            }

            _logger.LogInformation("Validation Finished {@Command}", typeName);

            return await next();
        }
    }
}