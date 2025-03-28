using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Abstractions;

public abstract class BaseHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected readonly ILogger Logger;
    protected readonly IMapper Mapper;
    protected readonly string RequestName;

    protected BaseHandler(IMapper mapper, ILogger logger)
    {
        Logger = logger;
        Mapper = mapper;
        RequestName = nameof(TRequest);
    }
}