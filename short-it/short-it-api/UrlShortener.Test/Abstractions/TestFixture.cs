using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace UrlShortener.Test.Abstractions;

public abstract class TestFixture<T> where T : class
{
    protected static readonly Fixture Fixture = new();
    protected static readonly Mock<IMapper> Mapper = new();
    protected static readonly Mock<ILogger<T>> Logger = new ();
    protected static readonly Mock<IMediator> Mediator = new();

    protected TestFixture()
    {
        
    }
}