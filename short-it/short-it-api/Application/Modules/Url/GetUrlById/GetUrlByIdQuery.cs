using Application.Commons.DTO;
using MediatR;

namespace Application.Modules.Url.GetUrlById;

public sealed record GetUrlByIdQuery(int UrlId) : IRequest<UrlResponse>;