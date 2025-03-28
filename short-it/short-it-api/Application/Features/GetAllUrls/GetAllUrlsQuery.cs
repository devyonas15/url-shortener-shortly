using Application.Commons.DTO;
using MediatR;

namespace Application.Features.GetAllUrls;

public sealed record GetAllUrlsQuery : IRequest<IReadOnlyList<UrlResponse>>;