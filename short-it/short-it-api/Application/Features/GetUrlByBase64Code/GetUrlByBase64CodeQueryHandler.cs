using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Constants;
using MediatR;

namespace Application.Features.GetUrlByBase64Code;

public sealed class GetUrlByBase64CodeQueryHandler :
    BaseHandlerWithValidator<GetUrlByBase64CodeQuery, UrlResponse, GetUrlByBase64CodeValidator>,
    IRequestHandler<GetUrlByBase64CodeQuery, UrlResponse>
{
    private readonly IUrlRepository _urlRepository;

    public GetUrlByBase64CodeQueryHandler(IUrlRepository urlRepository, IMapper mapper) : base(mapper)
    {
        _urlRepository = urlRepository;
    }

    public async Task<UrlResponse> Handle(GetUrlByBase64CodeQuery request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload:", validationResult);
            }

            var url = await _urlRepository.GetByShortUrlAsync($"{UrlConstants.BaseShortUrl}{request.Base64Code}",
                cancellationToken);

            if (url is null)
            {
                throw new NotFoundException($"Url not found for base64Code: {request.Base64Code}");
            }

            var response = Mapper.Map<UrlResponse>(url);

            return response;
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException or NotFoundException)
            {
                throw;
            }

            throw new Exception($"Error occured while processing GetUrlByBase64CodeQuery: {ex.Message}");
        }
    }
}