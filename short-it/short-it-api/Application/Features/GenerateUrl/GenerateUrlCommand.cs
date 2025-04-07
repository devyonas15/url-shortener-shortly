using Application.Commons.DTO;
using MediatR;

namespace Application.Features.GenerateUrl;

public sealed class GenerateUrlCommand : IRequest<GenerateUrlResponse>
{
    public required string LongUrl { get; set; }
}