using Application.Commons.DTO;
using MediatR;

namespace Application.Modules.Url.GetUrlByBase64Code;

public sealed record GetUrlByBase64CodeQuery(string Base64Code): IRequest<UrlResponse>;