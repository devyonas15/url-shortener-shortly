using Application.Abstractions;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Constants;
using Domain.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Modules.Url.GenerateUrl;

public sealed class GenerateUrlCommandHandler :
    BaseHandlerWithValidator<GenerateUrlCommand, GenerateUrlResponse, GenerateUrlCommandValidator>,
    IRequestHandler<GenerateUrlCommand, GenerateUrlResponse>
{
    private readonly IUrlRepository _urlRepository;

    public GenerateUrlCommandHandler(IUrlRepository urlRepository, IMapper mapper,
        ILogger<GenerateUrlCommandHandler> logger) : base(mapper, logger)
    {
        _urlRepository = urlRepository;
    }

    public async Task<GenerateUrlResponse> Handle(GenerateUrlCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Processing handler {handler} with request {@request}", RequestName, request);
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload when generating URL:", validationResult);
            }

            var existingUrl = await _urlRepository.GetByLongUrlAsync(request.LongUrl, cancellationToken);

            if (null != existingUrl)
            {
                throw new DuplicateException($"The url {request.LongUrl} already exists.");
            }

            var shortUrl = request.LongUrl.ToBase64Prefix();

            var url = new Domain.Entities.Url
            {
                ShortUrl = $"{UrlConstants.BaseShortUrl}{shortUrl}",
                LongUrl = request.LongUrl,
            };

            await _urlRepository.CreateAsync(url, cancellationToken);

            var response = Mapper.Map<GenerateUrlResponse>(url);

            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when generating the url due to: {message}", ex.Message);
            if (ex is DuplicateException or BadRequestException)
            {
                throw;
            }

            throw new Exception($"Error generating url: {ex.Message}");
        }
    }
}