using Application.Commons.DTO;
using MediatR;

namespace Application.Modules.Url.GetUrlsByUserId;

public sealed record GetUrlsByUserIdQuery(string UserId) : IRequest<IReadOnlyList<UrlResponse>>;