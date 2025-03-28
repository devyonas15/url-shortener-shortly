using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Abstractions;

public abstract class BaseHandlerWithValidator<TQuery, TResponse, TValidator> where TQuery : IRequest<TResponse>
    where TValidator : AbstractValidator<TQuery>, new()
{
    protected readonly IMapper Mapper;
    protected readonly TValidator Validator;

    protected BaseHandlerWithValidator(IMapper mapper)
    {
        Mapper = mapper;
        Validator = new TValidator();
    }
}