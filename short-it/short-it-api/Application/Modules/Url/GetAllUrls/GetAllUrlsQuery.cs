using Application.Commons.DTO;
using MediatR;

namespace Application.Modules.Url.GetAllUrls;

public sealed record GetAllUrlsQuery : IRequest<IReadOnlyList<UrlResponse>>;