using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogInformation("Handling {RequestName} {@Request}", requestName, request);

            var response = await next(cancellationToken);

            logger.LogInformation("Handled {RequestName} {@Response}", requestName, response);

            return response;
        }
    }
}
