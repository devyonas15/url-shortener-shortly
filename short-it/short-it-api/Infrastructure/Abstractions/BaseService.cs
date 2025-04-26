using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Abstractions;

public abstract class BaseService
{
    protected readonly ILogger Logger;
    protected readonly IMapper Mapper;

    protected BaseService(ILogger logger, IMapper mapper)
    {
        Logger = logger;
        Mapper = mapper;
    }
}