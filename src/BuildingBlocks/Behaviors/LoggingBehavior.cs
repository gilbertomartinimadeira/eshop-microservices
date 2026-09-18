namespace BuildingBlocks.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Logging logic before the request is handled
        _logger.LogInformation("[START] Handling {RequestName} - {Request}", typeof(TRequest).Name, request);
        var timer = System.Diagnostics.Stopwatch.StartNew(); 
        var response = await next();

        // Logging logic after the request is handled
        timer.Stop();

        var timeTaken = timer.Elapsed; 

        if(timeTaken.Seconds > 3)
        {
            _logger.LogWarning("[PERFORMANCE] Handling {RequestName} took {ElapsedMilliseconds}ms", typeof(TRequest).Name, timeTaken.TotalMilliseconds);
        }

        _logger.LogInformation("[END] Handled {RequestName} - {Response} in {ElapsedMilliseconds}ms", typeof(TRequest).Name, response, timer.ElapsedMilliseconds);

        return response;
    }
}