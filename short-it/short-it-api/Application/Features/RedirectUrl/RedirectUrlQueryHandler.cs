using Application.Abstractions;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.RedirectUrl;

public sealed class RedirectUrlQueryHandler : BaseHandlerWithValidator<RedirectUrlQuery, string, RedirectUrlQueryValidator>, IRequestHandler<RedirectUrlQuery, string>
{
    private readonly IUrlRepository _urlRepository;
    
    public RedirectUrlQueryHandler(IMapper mapper, ILogger<RedirectUrlQueryHandler> logger, IUrlRepository urlRepository) : base(mapper, logger)
    {
        _urlRepository = urlRepository;
    }

    public async Task<string> Handle(RedirectUrlQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation("Processing handler {handler} with request {@request}", RequestName, request);
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Request validation failed for payload:", validationResult);
            }
            
            var url = await _urlRepository.GetByShortUrlAsync($"{UrlConstants.BaseShortUrl}{request.Base64Code}", cancellationToken);

            if (url is null)
            {
                throw new NotFoundException($"Matched url not found for the short url: {request.Base64Code}");
            }

            return url.LongUrl;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when redirecting the url due to: {message}", ex.Message);
            
            if (ex is NotFoundException or BadRequestException)
            {
                throw;
            }
            
            throw new Exception($"Error occured while redirecting the URL: {ex.Message}");
        }
    }
}