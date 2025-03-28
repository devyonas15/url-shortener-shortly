using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Abstractions;

public abstract class BaseHandlerWithValidator<TRequest, TResponse, TValidator> where TRequest : IRequest<TResponse>
    where TValidator : AbstractValidator<TRequest>, new()
{
    protected readonly IMapper Mapper;
    protected readonly TValidator Validator;
    protected readonly ILogger Logger;
    protected readonly string RequestName;

    protected BaseHandlerWithValidator(IMapper mapper, ILogger logger)
    {
        Mapper = mapper;
        Validator = new TValidator();
        Logger = logger;
        RequestName = typeof(TRequest).Name;
    }
}