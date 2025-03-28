using AutoMapper;
using MediatR;

namespace Application.Abstractions;

public abstract class BaseHandler<TQuery, TResponse> where TQuery : IRequest<TResponse>
{
    protected readonly IMapper Mapper;

    protected BaseHandler(IMapper mapper)
    {
        Mapper = mapper;
    }
}