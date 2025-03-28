using AutoFixture;
using AutoMapper;
using MediatR;
using Moq;

namespace UrlShortener.Test.Abstractions;

public abstract class TestFixture
{
    protected readonly Fixture Fixture = new();
    protected static readonly Mock<IMapper> Mapper = new();
    protected static readonly Mock<IMediator> Mediator = new();

    protected TestFixture()
    {
        
    }
}