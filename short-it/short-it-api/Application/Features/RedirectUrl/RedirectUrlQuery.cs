using MediatR;

namespace Application.Features.RedirectUrl;

public sealed record RedirectUrlQuery(string Base64Code) : IRequest<string>;