using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse> 
            
            : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var context = new ValidationContext<TRequest>(request);

        var failures = await Task.WhenAll(_validators
            .Select(v => v.ValidateAsync(context)));

        var failureList = failures
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failureList.Count != 0)
        {
            throw new ValidationException(failureList);
        }
        return await next();
    }
}