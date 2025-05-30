using MediatR;

namespace Application.Modules.Url.GenerateUrl;

public sealed class GenerateUrlCommand : IRequest<GenerateUrlResponse>
{
    public required string LongUrl { get; set; }
}