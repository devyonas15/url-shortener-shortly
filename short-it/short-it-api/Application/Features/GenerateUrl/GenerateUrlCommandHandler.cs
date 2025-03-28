using Application.Abstractions;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Extensions;
using MediatR;

namespace Application.Features.GenerateUrl;

public sealed class GenerateUrlCommandHandler: BaseHandlerWithValidator<GenerateUrlCommand, int, GenerateUrlCommandValidator>, IRequestHandler<GenerateUrlCommand, int>
{
    private readonly IUrlRepository _urlRepository;

    public GenerateUrlCommandHandler(IUrlRepository urlRepository, IMapper mapper) : base(mapper)
    {
        _urlRepository = urlRepository;
    }

    public async Task<int> Handle(GenerateUrlCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
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

            var url = new Url
            {
                ShortUrl = $"{UrlConstants.BaseShortUrl}{shortUrl}",
                LongUrl = request.LongUrl,
            };

            await _urlRepository.CreateAsync(url, cancellationToken);

            return url.UrlId;
        }
        catch (Exception ex)
        {
            if (ex is DuplicateException or BadRequestException)
            {
                throw;
            }

            throw new Exception($"Error generating url: {ex.Message}");
        }
    }
}