using MediatR;

namespace Application.Features.GenerateUrl;

public sealed class GenerateUrlCommand : IRequest<int>
{
    public required string LongUrl { get; set; }
}