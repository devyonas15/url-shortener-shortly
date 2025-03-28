using Application.Commons.DTO;
using MediatR;

namespace Application.Features.GetUrlByBase64Code;

public sealed record GetUrlByBase64CodeQuery(string Base64Code): IRequest<UrlResponse>;