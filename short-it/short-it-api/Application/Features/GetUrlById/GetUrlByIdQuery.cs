using Application.Commons.DTO;
using MediatR;

namespace Application.Features.GetUrlById;

public sealed record GetUrlByIdQuery(int UrlId) : IRequest<UrlResponse>;