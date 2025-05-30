using MediatR;

namespace Application.Modules.Url.RedirectUrl;

public sealed record RedirectUrlQuery(string Base64Code) : IRequest<string>;