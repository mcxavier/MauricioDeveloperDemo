﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Infra.EntitityConfigurations.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Utils.Extensions;

namespace Infra.QueryCommands._Kernel.Behaviors
{

    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

        private readonly CoreContext _dbContext;

        public TransactionBehaviour(CoreContext dbContext, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {

            _dbContext = dbContext ?? throw new ArgumentException(nameof(CoreContext));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);

            var typeName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {

                    await using var transaction = await _dbContext.BeginTransactionAsync();

                    using var property = LogContext.PushProperty("TransactionContext", transaction.TransactionId);

                    _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);
                    response = await next();
                    _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _dbContext.CommitTransactionAsync(transaction);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}